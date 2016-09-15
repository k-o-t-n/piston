namespace Piston
{
    using Autofac;
    using Autofac.Integration.Mvc;
    using Piston.Markdown;
    using Storage;

    public static class AutofacBootstrapper
    {
        public static IContainer Initialize()
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            builder.RegisterType<LocalPageStorage>().Named<IPageStorage>("pageStorage").SingleInstance();
            builder.RegisterType<LocalPostStorage>().Named<IPostStorage>("postStorage").SingleInstance();

            builder.RegisterDecorator<IPageStorage>((c, inner) => new CachedPageStorage(inner), fromKey: "pageStorage").SingleInstance();
            builder.RegisterDecorator<IPostStorage>((c, inner) => new CachedPostStorage(inner), fromKey: "postStorage").SingleInstance();

            builder.RegisterType<MetadataMarkdown>().SingleInstance();

            return builder.Build();
        }
    }
}