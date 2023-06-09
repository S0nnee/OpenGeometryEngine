namespace OpenGeometryEngine.Structures;

/// <summary>
/// Represents an intersection point between two curves.
/// </summary>
public readonly struct IntersectionPoint<TEvalA, TEvalB>
{
    /// <summary>
    /// Gets the evaluation of the first curve at the intersection point.
    /// </summary>
    public readonly TEvalA FirstCurveEvaluation { get; }

    /// <summary>
    /// Gets the evaluation of the second curve at the intersection point.
    /// </summary>
    public readonly TEvalB SecondCurveEvaluation { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="IntersectionPoint"/> class.
    /// </summary>
    /// <param><c>xPosition</c> is the new Point's x-coordinate.</param>
    /// <param name="curveEval1">The evaluation of the first curve at the intersection point.</param>
    /// <param name="curveEval2">The evaluation of the second curve at the intersection point.</param>
    internal IntersectionPoint(TEvalA curveEval1, TEvalB curveEval2) =>
        (FirstCurveEvaluation, SecondCurveEvaluation) = (curveEval1, curveEval2);

}
