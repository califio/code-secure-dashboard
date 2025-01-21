using CodeSecure.Manager.Notification.Model;
using CodeSecure.Manager.Project.Model;
using Fluid;
using Fluid.Values;

namespace CodeSecure.Manager.Notification;

public static class TemplateEngine
{
    private static readonly FluidParser Parser = new();
    private static TemplateOptions? options;

    public static string Render(string template, object? model)
    {
        if (options == null)
        {
            options = new TemplateOptions();
            options.MemberAccessStrategy.Register<DependencyProject>();
            options.MemberAccessStrategy.Register<FindingModel>();
        }
        if (!Parser.TryParse(template, out var engine, out var error)) throw new ParseException(error);
        model ??= NilValue.Instance;
        var context = new TemplateContext(model, options);
        return engine.Render(context);
    }
}