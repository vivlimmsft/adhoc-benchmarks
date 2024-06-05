using BenchmarkDotNet.Attributes;
using System.Collections.Immutable;

namespace dotnet_benchmarks_scratch;

[MemoryDiagnoser]
public class AsyncInitializingProperties
{
    [Params(10000)]
    public static int N;

    [Benchmark]
    public async Task EachPropertyNullableAsync()
    {
        var initializedClass = new EachPropertyNullable();
        await initializedClass.InitializeAsync();
        initializedClass.Invoke();
    }

    [Benchmark]
    public async Task AsyncInitPropertyRecordAsync()
    {
        var initializedClass = new AsyncInitPropertyRecord();
        await initializedClass.InitializeAsync();
        initializedClass.Invoke();
    }

    [Benchmark]
    public async Task AsyncInitPropertyNullableRecordStructAsync()
    {
        var initializedClass = new AsyncInitPropertyNullableRecordStruct();
        await initializedClass.InitializeAsync();
        initializedClass.Invoke();
    }

    [Benchmark]
    public async Task AsyncInitPropertyNotNullableRecordStructAsync()
    {
        var initializedClass = new AsyncInitPropertyNotNullableRecordStruct();
        await initializedClass.InitializeAsync();
        initializedClass.Invoke();
    }

    public interface IAsyncInitializer
    {
        Task InitializeAsync();

        void Invoke();
    }

    public class EachPropertyNullable : IAsyncInitializer
    {
        private InnerImplementation? innerImplementation;

        public async Task InitializeAsync()
        {
            this.innerImplementation = await InnerImplementation.FactoryAsync();
        }

        public void Invoke()
        {
            for (int i = 0; i < N; i++)
            {
                this.innerImplementation?.Echo(i);
            }
        }
    }

    public class AsyncInitPropertyRecord : IAsyncInitializer
    {
        private AsyncInitProperties? asyncInitProperties;

        public async Task InitializeAsync()
        {
            var innerImplementation = await InnerImplementation.FactoryAsync();
            this.asyncInitProperties = new AsyncInitProperties(innerImplementation);
        }

        public void Invoke()
        {
            for (int i = 0; i < N; i++)
            {
                this.asyncInitProperties?.innerImplementation.Echo(i);
            }
        }

        private record AsyncInitProperties(InnerImplementation innerImplementation);
    }

    public class AsyncInitPropertyNullableRecordStruct : IAsyncInitializer
    {
        private AsyncInitProperties? asyncInitProperties;

        public async Task InitializeAsync()
        {
            var innerImplementation = await InnerImplementation.FactoryAsync();
            this.asyncInitProperties = new AsyncInitProperties(innerImplementation);
        }

        public void Invoke()
        {
            for (int i = 0; i < N; i++)
            {
                this.asyncInitProperties?.innerImplementation.Echo(i);
            }
        }

        private record struct AsyncInitProperties(InnerImplementation innerImplementation);
    }

    public class AsyncInitPropertyNotNullableRecordStruct : IAsyncInitializer
    {
        private AsyncInitProperties asyncInitProperties;

        public async Task InitializeAsync()
        {
            var innerImplementation = await InnerImplementation.FactoryAsync();
            this.asyncInitProperties = new AsyncInitProperties(innerImplementation);
        }

        public void Invoke()
        {
            for (int i = 0; i < N; i++)
            {
                this.asyncInitProperties.innerImplementation.Echo(i);
            }
        }

        private record struct AsyncInitProperties(InnerImplementation innerImplementation);
    }


    public class InnerImplementation
    {
        private InnerImplementation()
        {
        }

        public async static Task<InnerImplementation> FactoryAsync()
        {
            await Task.Yield();
            return new InnerImplementation();
        }

        public int Echo(int start)
        {
            return start;
        }
    }
}
