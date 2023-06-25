using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Automation;

public class ControlHighlighter
{
    private List<AutomationElement> highlightedElements;
    private Pen highlightPen;

    public ControlHighlighter()
    {
        // Initialize the list of highlighted elements
        highlightedElements = new List<AutomationElement>();
        // Initialize the highlight pen
        highlightPen = new Pen(Color.Red, 2);
    }

    public void HighlightControls()
    {

        if ((AutomationElement.FocusedElement != null
            ? AutomationElement.FromHandle(new IntPtr(AutomationElement.FocusedElement.Current.NativeWindowHandle))
            : null) != null && !highlightedElements.Contains(AutomationElement.FocusedElement != null
            ? AutomationElement.FromHandle(new IntPtr(AutomationElement.FocusedElement.Current.NativeWindowHandle))
            : null))
        {
            // Clear the previous highlights
            ClearHighlights();

            // Traverse the visual tree and highlight each element
            TraverseAndHighlightElements(AutomationElement.FocusedElement != null
            ? AutomationElement.FromHandle(new IntPtr(AutomationElement.FocusedElement.Current.NativeWindowHandle))
            : null);

            // Add the current window to the list of highlighted elements
            highlightedElements.Add(AutomationElement.FocusedElement != null
            ? AutomationElement.FromHandle(new IntPtr(AutomationElement.FocusedElement.Current.NativeWindowHandle))
            : null);
        }
    }

    private void TraverseAndHighlightElements(AutomationElement element)
    {
        // Highlight the current element
        RedrawElement(element);

        // Traverse the children elements recursively
        AutomationElementCollection children = element.FindAll(TreeScope.Children, Condition.TrueCondition);
        foreach (AutomationElement childElement in children)
        {
            TraverseAndHighlightElements(childElement);
        }
    }

    private void RedrawElement(AutomationElement element)
    {
        Rect bounds = element.Current.BoundingRectangle;
        IntPtr handle = new IntPtr(element.Current.NativeWindowHandle);
        using (Graphics graphics = Graphics.FromHwnd(handle))
        {
            RectangleF rect = new RectangleF((float)bounds.Left, (float)bounds.Top, (float)bounds.Width, (float)bounds.Height);
            graphics.DrawRectangle(highlightPen, rect.Left, rect.Top, rect.Width, rect.Height);
        }
    }

    public static void Main()
    {
        ControlHighlighter highlighter = new ControlHighlighter();

        Console.WriteLine("Press Enter to stop the program...");

        while (!Console.KeyAvailable)
        {
            highlighter.HighlightControls();
            System.Threading.Thread.Sleep(100);
        }

        // Clear the highlights before exiting
        highlighter.ClearHighlights();
    }

    public void ClearHighlights()
    {
        foreach (AutomationElement element in highlightedElements)
        {
            RedrawElement(element);
        }
        highlightedElements.Clear();
    }
}
