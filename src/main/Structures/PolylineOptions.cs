﻿namespace OpenGeometryEngine;

public readonly struct PolylineOptions
{
    public readonly double AngularDeviation;

    public readonly double MaxChordLength;

    public readonly double ChordTolerance;

    public static readonly PolylineOptions Default = 
        new (DefaultAngularDeviation, DefaultMaxChordLength, DefaultChordTolerance);

    public PolylineOptions(double angularDeviation, double maxChordLength, double chordTolerance)
    {
        AngularDeviation = angularDeviation;
        MaxChordLength = maxChordLength;
        ChordTolerance = chordTolerance;
    }

    public const double DefaultAngularDeviation = 0.15;

    public const double DefaultMaxChordLength = 0.0002;

    public const double DefaultChordTolerance = 1E-5;
}