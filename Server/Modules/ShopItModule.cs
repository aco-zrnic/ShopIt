using Autofac;
using Server.Services;

namespace Server.Modules
{
    public class ShopItModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AwsS3Service>().As<IAwsS3Service>().InstancePerDependency();
        }
    }
}
