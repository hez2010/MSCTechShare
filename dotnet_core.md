# 关于 .net core 2.1
1、.net framework 的超集，目前 2.1 版本还在预览版状态，预计几个月后正式发布。

2、性能强于 .net framework，每一个底层类库都被重写，紧扣内存分配和性能的实现，在 Web 方面性能仅次于 Go 和 C。

3、跨平台，支持 Windows x86, x64, ARM, Linux x86, x64, ARM, MacOS x64

4、可以作为各种前端框架的后端来使用，如 Angular、Knockout、Vue、React 等等

5、编译器优化

考虑如下代码：
```cs
using System;

class Y
{
    static int Main()
    {
        var s = new X<string>("hello, world!");
        ((IPrint)s).Print();
        return 100;
    }
}
 
interface IPrint { void Print(); }
 
struct X<T> : IPrint
{
    T _t;
    public X(T t) => _t = t;
    public void Print() => Console.WriteLine(_t);
}
```
.net core 2.0 会将其编译为
```asm
// Main method...
push     ebp
mov      ebp, esp
push     esi
mov      esi, gword ptr [0875203CH] 'hello, world!'
mov      ecx, 0x59E4D3C
call     CORINFO_HELP_NEWSFAST
mov      ecx, eax
lea      edx, bword ptr [ecx+4]
call     CORINFO_HELP_ASSIGN_REF_ESI
call     [IPrint:Print():this]
mov      eax, 100
pop      esi
pop      ebp
ret

// ...which calls to X<T>.Print:
push     ebp
mov      ebp, esp
push     eax
mov      dword ptr [ebp-04H], edx
mov      ecx, gword ptr [ecx]
call     [System.Console:WriteLine(ref)]
mov      esp, ebp
pop      ebp
ret
```

.net core 2.1 会将其编译为：
```asm
mov      ecx, gword ptr [0859203CH] 'hello, world!'
call     [System.Console:WriteLine(ref)]
mov      eax, 100
ret
```