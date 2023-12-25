using Autofac;

namespace Server.Behavior
{
    public class BehaviorModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<LoggingActionFilterBehavior>();
        }
    }
}
