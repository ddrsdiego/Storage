``` ini

BenchmarkDotNet=v0.13.2, OS=ubuntu 20.04
Intel Core i7-10510U CPU 1.80GHz, 1 CPU, 8 logical and 4 physical cores
.NET SDK=6.0.202
  [Host] : .NET 6.0.4 (6.0.422.16404), X64 RyuJIT AVX2


```
|         Method | Mean | Error |
|--------------- |-----:|------:|
| DownloadString |   NA |    NA |

Benchmarks with issues:
  WebClientHelper.DownloadString: DefaultJob
