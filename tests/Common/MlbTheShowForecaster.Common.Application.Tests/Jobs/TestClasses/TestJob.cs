using com.brettnamba.MlbTheShowForecaster.Common.Application.Jobs;

namespace com.brettnamba.MlbTheShowForecaster.Common.Application.Tests.Jobs.TestClasses;

public sealed class TestJob : BaseJob<TestJobInput, TestJobOutput>
{
    private readonly int _msDelay;
    private TaskCompletionSource<TestJobOutput> _tcs;
    private int _runCount;

    public TestJob(int msDelay, TaskCompletionSource<TestJobOutput> tcs)
    {
        _msDelay = msDelay;
        _tcs = tcs;
    }

    public override async Task<TestJobOutput> Execute(TestJobInput input, CancellationToken cancellationToken = default)
    {
        await Task.Delay(_msDelay, cancellationToken);

        _runCount++;
        var result = new TestJobOutput($"Finished {input.Input}. Count: {_runCount}");
        _tcs.SetResult(result);
        return result;
    }

    public void ResetTaskCompletionSource()
    {
        _tcs = new TaskCompletionSource<TestJobOutput>();
    }

    public TaskCompletionSource<TestJobOutput> GetTaskCompletionSource()
    {
        return _tcs;
    }
}

public sealed class TestExceptionJob : BaseJob<TestJobInput, TestJobOutput>
{
    public static readonly Exception JobException = new Exception("Job failed");

    public override Task<TestJobOutput> Execute(TestJobInput input, CancellationToken cancellationToken = default)
    {
        throw JobException;
    }
}

public sealed record TestJobInput(string Input) : IJobInput;

public sealed record TestJobOutput(string Output) : IJobOutput;