using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Components.RenderTree;

/// <summary>
/// This file is based on
/// https://github.com/egil/bUnit/blob/main/src/bunit.web/Rendering/Internal/Htmlizer.cs
/// </summary>
static class Htmlizer
{
    static readonly HtmlEncoder HtmlEncoder = HtmlEncoder.Default;

    static readonly HashSet<string> SelfClosingElements = new(StringComparer.OrdinalIgnoreCase)
    {
        "area", "base", "br", "col", "embed", "hr", "img", "input", "link", "meta", "param", "source", "track", "wbr"
    };

    const string BLAZOR_INTERNAL_ATTR_PREFIX = "__internal_";
    const string BLAZOR_ATTR_PREFIX = "blazor:";
    const string ELEMENT_REFERENCE_ATTR_NAME = BLAZOR_ATTR_PREFIX + "elementReference";

    public static string GetHtml(TestRenderer renderer, int componentId)
    {
        var frames = renderer.GetCurrentRenderTreeFrames(componentId);
        HtmlRenderingContext context = new(renderer);
        RenderFrames(context, frames, 0, frames.Count);
        return context.Result.ToString();
    }

    static int RenderFrames(HtmlRenderingContext context, ArrayRange<RenderTreeFrame> frames, int position, int maxElements)
    {
        var nextPosition = position;
        var endPosition = position + maxElements;
        while (position < endPosition)
        {
            nextPosition = RenderCore(context, frames, position);
            if (position == nextPosition)
            {
                throw new("We didn't consume any input.");
            }
            position = nextPosition;
        }

        return nextPosition;
    }

    static int RenderCore(
        HtmlRenderingContext context,
        ArrayRange<RenderTreeFrame> frames,
        int position)
    {
        ref var frame = ref frames.Array[position];
        switch (frame.FrameType)
        {
            case RenderTreeFrameType.Element:
                return RenderElement(context, frames, position);
            case RenderTreeFrameType.Attribute:
                throw new($"Attributes should only be encountered within {nameof(RenderElement)}");
            case RenderTreeFrameType.Text:
                context.Result.Append(HtmlEncoder.Encode(frame.TextContent));
                return ++position;
            case RenderTreeFrameType.Markup:
                context.Result.Append(frame.MarkupContent);
                return ++position;
            case RenderTreeFrameType.Component:
                return RenderChildComponent(context, frames, position);
            case RenderTreeFrameType.Region:
                return RenderFrames(context, frames, position + 1, frame.RegionSubtreeLength - 1);
            case RenderTreeFrameType.ElementReferenceCapture:
            case RenderTreeFrameType.ComponentReferenceCapture:
                return ++position;
            default:
                throw new($"Invalid element frame type '{frame.FrameType}'.");
        }
    }

    static int RenderChildComponent(
        HtmlRenderingContext context,
        ArrayRange<RenderTreeFrame> frames,
        int position)
    {
        ref var frame = ref frames.Array[position];
        var childFrames = context.Renderer.GetCurrentRenderTreeFrames(frame.ComponentId);
        RenderFrames(context, childFrames, 0, childFrames.Count);
        return position + frame.ComponentSubtreeLength;
    }

    static int RenderElement(
        HtmlRenderingContext context,
        ArrayRange<RenderTreeFrame> frames,
        int position)
    {
        ref var frame = ref frames.Array[position];
        var result = context.Result;
        result.Append("<");
        result.Append(frame.ElementName);
        var afterAttributes = RenderAttributes(context, frames, position + 1, frame.ElementSubtreeLength - 1, out var capturedValueAttribute);

        // When we see an <option> as a descendant of a <select>, and the option's "value" attribute matches the
        // "value" attribute on the <select>, then we auto-add the "selected" attribute to that option. This is
        // a way of converting Blazor's select binding feature to regular static HTML.
        if (context.ClosestSelectValueAsString != null
            && string.Equals(frame.ElementName, "option", StringComparison.OrdinalIgnoreCase)
            && string.Equals(capturedValueAttribute, context.ClosestSelectValueAsString, StringComparison.Ordinal))
        {
            result.Append(" selected");
        }

        var remainingElements = frame.ElementSubtreeLength + position - afterAttributes;
        if (remainingElements > 0)
        {
            result.Append(">");

            var isSelect = string.Equals(frame.ElementName, "select", StringComparison.OrdinalIgnoreCase);
            if (isSelect)
            {
                context.ClosestSelectValueAsString = capturedValueAttribute;
            }

            var afterElement = RenderChildren(context, frames, afterAttributes, remainingElements);

            if (isSelect)
            {
                // There's no concept of nested <select> elements, so as soon as we're exiting one of them,
                // we can safely say there is no longer any value for this
                context.ClosestSelectValueAsString = null;
            }

            result.Append("</");
            result.Append(frame.ElementName);
            result.Append(">");
            return afterElement;
        }
        else
        {
            if (SelfClosingElements.Contains(frame.ElementName))
            {
                result.Append(" />");
            }
            else
            {
                result.Append("></");
                result.Append(frame.ElementName);
                result.Append(">");
            }
            return afterAttributes;
        }
    }

    static int RenderChildren(HtmlRenderingContext context, ArrayRange<RenderTreeFrame> frames, int position, int maxElements)
    {
        if (maxElements == 0)
        {
            return position;
        }

        return RenderFrames(context, frames, position, maxElements);
    }

    static int RenderAttributes(
        HtmlRenderingContext context,
        ArrayRange<RenderTreeFrame> frames, int position, int maxElements, out string? capturedValueAttribute)
    {
        capturedValueAttribute = null;

        if (maxElements == 0)
        {
            return position;
        }

        var result = context.Result;

        for (var i = 0; i < maxElements; i++)
        {
            var candidateIndex = position + i;
            ref var frame = ref frames.Array[candidateIndex];

            // Added to write ElementReferenceCaptureId to DOM
            if (frame.FrameType == RenderTreeFrameType.ElementReferenceCapture)
            {
                result.Append($" {ELEMENT_REFERENCE_ATTR_NAME}=\"{frame.ElementReferenceCaptureId}\"");
            }

            if (frame.FrameType != RenderTreeFrameType.Attribute)
            {
                return candidateIndex;
            }

            if (frame.AttributeName.Equals("value", StringComparison.OrdinalIgnoreCase))
            {
                capturedValueAttribute = frame.AttributeValue as string;
            }

            if (frame.AttributeEventHandlerId > 0)
            {
                // NOTE: this was changed from
                //       result.Add($" {frame.AttributeName}=\"{frame.AttributeEventHandlerId}\"");
                //       to the following to make it more obvious
                //       that this is a generated/special blazor attribute
                //       used for tracking event handler id's
                result.Append(" ");
                result.Append(BLAZOR_ATTR_PREFIX);
                result.Append(frame.AttributeName);
                result.Append("=\"");
                result.Append(frame.AttributeEventHandlerId.ToString(CultureInfo.InvariantCulture));
                result.Append("\"");
                continue;
            }

            switch (frame.AttributeValue)
            {
                case bool flag when flag && frame.AttributeName.StartsWith(BLAZOR_INTERNAL_ATTR_PREFIX, StringComparison.Ordinal):
                    // NOTE: This was added to make it more obvious
                    //       that this is a generated/special blazor attribute
                    //	     for internal usage
                    var nameParts = frame.AttributeName.Split('_', StringSplitOptions.RemoveEmptyEntries);
                    result.Append(" ");
                    result.Append(BLAZOR_ATTR_PREFIX);
                    result.Append(BLAZOR_ATTR_PREFIX);
                    result.Append(frame.AttributeName);
                    result.Append(nameParts[2]);
                    result.Append(":");
                    result.Append(nameParts[1]);
                    break;
                case bool flag when flag:
                    result.Append(" ");
                    result.Append(frame.AttributeName);
                    break;
                case string value:
                    result.Append(" ");
                    result.Append(frame.AttributeName);
                    result.Append("=\"");
                    result.Append(HtmlEncoder.Encode(value));
                    result.Append("\"");
                    break;
            }
        }

        return position + maxElements;
    }

    class HtmlRenderingContext
    {
        public TestRenderer Renderer { get; }

        public HtmlRenderingContext(TestRenderer renderer)
        {
            Renderer = renderer;
        }

        public StringBuilder Result { get; } = new();

        public string? ClosestSelectValueAsString { get; set; }
    }
}