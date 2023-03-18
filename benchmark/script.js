import http from 'k6/http';
import { check, sleep } from 'k6';

export const options = {
  stages: [
	{ duration: '30s', target: 50 },
	{ duration: '20s', target: 50 },
	{ duration: '10s', target: 75 },
	{ duration: '10s', target: 100 },
	{ duration: '10s', target: 50 },
	{ duration: '10s', target: 0 },
  ],
  noConnectionReuse: true,
  thresholds: {
    http_req_failed: ['rate<0.01'], // http errors should be less than 1%
    http_req_duration: ['p(95)<100'], // 95% of requests should be below 200ms
  },
};

export default function () {
    
	const req1 = {
		method: 'GET',
		url: 'http://localhost:5093/customers/000001',
	};

	const req2 = {
		method: 'GET',
		url: 'http://localhost:5093/customers/000002',
	};

	const req3 = {
		method: 'GET',
		url: 'http://localhost:5093/customers/000003',
	};

	const req4 = {
		method: 'GET',
		url: 'http://localhost:5093/customers/000004',
	};

	const req5 = {
		method: 'GET',
		url: 'http://localhost:5093/customers/000100',
	};

	const responses = http.batch([req1, req2, req3, req4, req5]);

	check(responses[0], {
		'main page 200': (res) => res.status === 200,
		'pi page has right content': (res) => console.log('Response time was ' + String(res.timings.duration) + ' ms'),
	});
}