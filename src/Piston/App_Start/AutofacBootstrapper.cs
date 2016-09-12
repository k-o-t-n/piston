using Autofac;
using Autofac.Integration.Mvc;
using HeyRed.MarkdownSharp;
using Piston.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Piston
{
    public static class AutofacBootstrapper
    {
        public static IContainer Initialize()
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            builder.RegisterType<PageStorage>().Named<IPageStorage>("pageStorage").SingleInstance();
            builder.RegisterType<PostStorage>().Named<IPostStorage>("postStorage").SingleInstance();

            builder.RegisterDecorator<IPageStorage>((c, inner) => new CachedPageStorage(inner), fromKey: "pageStorage").SingleInstance();
            builder.RegisterDecorator<IPostStorage>((c, inner) => new CachedPostStorage(inner), fromKey: "postStorage").SingleInstance();

            builder.RegisterType<Markdown>().SingleInstance();

            return builder.Build();
        }
    }
}