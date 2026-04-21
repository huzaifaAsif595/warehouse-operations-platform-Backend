using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PeakLogix.NetCoreLib.Util;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PeakLogix.PickProApi.Middlewares;

public class RequestLogger(RequestDelegate next, ILogger<RequestLogger> logger)
{

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        
        // create our own cancellation token and we want to link it to the request's cancellation token
        var cts = new CancellationTokenSource();
        context.RequestAborted.Register(() => cts.Cancel());



        Task logSlowRequests = Task.Run(async () =>
        {
            int counter = 0;
            try {
                while (cts.Token.IsCancellationRequested == false)
                {
                    await Task.Delay(1000, cts.Token);
                    if (cts.Token.IsCancellationRequested == false)
                    {
                        // log after 1 sec the first time, then after 10 seconds after that
                        if (counter == 0 || counter % 10 == 0)
                        {
                            logger.LogWarning("Request for {Path} has been running for {Elapsed} seconds", context.Request.Path, stopwatch.Elapsed.TotalSeconds);
                        }
                    }
                    counter++;
                }
            }
            catch (OperationCanceledException)
            {
                // we are done
            }

        }, cts.Token);


        var request = context.Request;
        var clientIPv4 = request.Headers["X-Forwarded-For"].FirstOrDefault() ??
            context.Connection.RemoteIpAddress?.MapToIPv4().ToString() ?? "IPv4 unknown";
        var clientIPv6 = request.Headers["X-Forwarded-For"].FirstOrDefault() ??
            context.Connection.RemoteIpAddress?.MapToIPv6().ToString() ?? "IPv6 unknown";
        var userAgent = request.Headers.UserAgent.FirstOrDefault() ?? "unknown";
        try
        {
            await next(context);
        }
        finally
        {
            await cts.CancelAsync();
        }
        var response = context.Response;
        var statusCode = response.StatusCode;
        var responseLength = response.ContentLength ?? 0;
        stopwatch.Stop();
        var elapsedTime = stopwatch.Elapsed;

        var logMessage = $"{clientIPv4} {clientIPv6} {userAgent} {request.Method} {request.Path} {statusCode} {responseLength}  - Elapsed time: {elapsedTime}";
        if (statusCode == 200)
        {
            logger.LogInformation("{}", logMessage);
        }
        else
        {
            logger.LogWarning("{}", logMessage);
        }
    }
}