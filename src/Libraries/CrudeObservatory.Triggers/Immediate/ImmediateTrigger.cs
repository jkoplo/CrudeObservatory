﻿using CrudeObservatory.Abstractions.Interfaces;

namespace CrudeObservatory.Triggers.Immediate
{
    public class ImmediateTrigger : ITrigger
    {
        public Task InitializeAsync(CancellationToken stoppingToken) => Task.CompletedTask;

        public Task ShutdownAsync(CancellationToken stoppingToken) => Task.CompletedTask;

        public Task WaitForTriggerAsync(CancellationToken stoppingToken) => Task.CompletedTask;
    }
}
