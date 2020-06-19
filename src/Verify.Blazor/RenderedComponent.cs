using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace VerifyTests.Blazor
{
    public class RenderedComponent<TComponent>
        where TComponent : class, IComponent
    {
        TestRenderer renderer;
        ContainerComponent root;
        int testComponentId;

        public RenderedComponent(TestRenderer renderer)
        {
            this.renderer = renderer;
            root = new ContainerComponent(this.renderer);
        }

        public TComponent Instance { get; private set; } = null!;

        public string GetMarkup()
        {
            return Htmlizer.GetHtml(renderer, testComponentId);
        }

        public async Task SetParametersAndRender(ParameterView parameters)
        {
            await root.RenderComponentUnderTest<TComponent>(parameters);
            var (componentId, component) = root.FindComponentUnderTest<TComponent>();
            testComponentId = componentId;
            Instance = component;
        }
    }
}