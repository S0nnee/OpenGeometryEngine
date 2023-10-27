using static System.Math;

using System;
using System.Text;

namespace OpenGeometryEngine;

public readonly struct Matrix
{
    private readonly double[,] _data;

    public Matrix() => _data = new double[4, 4];

    public static Matrix CreateScale(double sx, double sy, double sz)
    {
        return new Matrix
        {
            _data =
            {
                [0, 0] = sx,
                [1, 1] = sy,
                [2, 2] = sz,
                [3, 3] = 1f
            }
        };
    }

    public static Matrix CreateTranslation(double dx, double dy, double dz)
    {
        return new Matrix
        {
            _data =
            {
                [0, 0] = 1f,
                [1, 1] = 1f,
                [2, 2] = 1f,
                [3, 3] = 1f,
                [0, 3] = dx,
                [1, 3] = dy,
                [2, 3] = dz,
            }
        };
    }

    public static Matrix CreateRotation(Vector direction, double angle)
    {
        // Normalize the axis vector
        Vector unitDir = direction.Normalize();

        // Precompute trigonometric values
        float cos = (float)Math.Cos(angle);
        float sin = (float)Math.Sin(angle);
        float oneMinusCos = 1f - cos;

        // Calculate the rotation matrix elements
        double xx = unitDir.X * unitDir.X;
        double xy = unitDir.X * unitDir.Y;
        double xz = unitDir.X * unitDir.Z;
        double yy = unitDir.Y * unitDir.Y;
        double yz = unitDir.Y * unitDir.Z;
        double zz = unitDir.Z * unitDir.Z;
        double lxy = xy * oneMinusCos;
        double lyz = yz * oneMinusCos;
        double lzx = xz * oneMinusCos;
        double xSin = unitDir.X * sin;
        double ySin = unitDir.Y * sin;
        double zSin = unitDir.Z * sin;
            
        return new Matrix
        {
            _data =
            {
                [0, 0] = cos + xx * oneMinusCos,
                [0, 1] = lxy - zSin,
                [0, 2] = lzx + ySin,
                [0, 3] = 0f,

                [1, 0] = lxy + zSin,
                [1, 1] = cos + yy * oneMinusCos,
                [1, 2] = lyz - xSin,
                [1, 3] = 0f,

                [2, 0] = lzx - ySin,
                [2, 1] = lyz + xSin,
                [2, 2] = cos + zz * oneMinusCos,
                [2, 3] = 0f,

                [3, 0] = 0f,
                [3, 1] = 0f,
                [3, 2] = 0f,
                [3, 3] = 1f
            }
        };
    }

    private static Matrix CreateRotation(Vector xAxis, Vector yAxis, Vector zAxis)
    {
        var world = Frame.World;
        var alp1 = world.DirX.Angle(xAxis);
        var betha1 = world.DirY.Angle(xAxis);
        var gamma1 = world.DirZ.Angle(xAxis);

        var alp2 = world.DirX.Angle(yAxis);
        var betha2 = world.DirY.Angle(yAxis);
        var gamma2 = world.DirZ.Angle(yAxis);

        var alp3 = world.DirX.Angle(zAxis);
        var betha3 = world.DirY.Angle(zAxis);
        var gamma3 = world.DirZ.Angle(zAxis);


        return new Matrix
        {
            _data =
            {
                [0, 0] = Cos(alp1),
                [0, 1] = Cos(betha1),
                [0, 2] = Cos(gamma1),
                [0, 3] = 0f,

                [1, 0] = Cos(alp2),
                [1, 1] = Cos(betha2),
                [1, 2] = Cos(gamma2),
                [1, 3] = 0f,

                [2, 0] = Cos(alp3),
                [2, 1] = Cos(betha3),
                [2, 2] = Cos(gamma3),
                [2, 3] = 0f,

                [3, 0] = 0f,
                [3, 1] = 0f,
                [3, 2] = 0f,
                [3, 3] = 1f
            }
        };
    }


    public static Point operator *(Matrix transformationMatrix, Point point)
    {
        var pointData = new[] { point.X, point.Y, point.Z, 1 };
        Multiply(transformationMatrix, ref pointData);
        return new Point(pointData[0], pointData[1], pointData[2]);
    }

    public static Vector operator *(Matrix transformationMatrix, Vector vector)
    {
        var vectorData = new[] { vector.X, vector.Y, vector.Z, 0 };
        Multiply(transformationMatrix, ref vectorData);
        return new Vector(vectorData[0], vectorData[1], vectorData[2]);
    }

    private static void Multiply(Matrix matrix, ref double[] data)
    {
        if (data.Length != 4) throw new ArgumentException("data length should be 4.");
        var newPointAsArray = new double[4];
        for (int i = 0; i < 4; i++)
        {
            var sum = 0d;
            for (int j = 0; j < 4; j++) sum += matrix._data[i, j] * data[j];
            newPointAsArray[i] = sum;
        }
        data = newPointAsArray;
    }

    public static Matrix operator *(Matrix matrix1, Matrix matrix2)
    {
        var result = new Matrix();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                double sum = 0.0;
                for (int k = 0; k < 4; k++)
                {
                    sum += matrix1._data[i, k] * matrix2._data[k, j];
                }
                result._data[i, j] = sum;
            }
        }
        return result;
    }

    public static Matrix CreateMapping(Frame frame)
    {
        var world = Frame.World;
        var rot = CreateRotation(frame.DirX, frame.DirY, frame.DirZ).Transpose();
        var trans = CreateTranslation(frame.Origin.X, frame.Origin.Y, frame.Origin.Z);
        return trans * rot;
    }

    public Matrix Transpose()
    {
        var transposedMatrix = new Matrix();

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                transposedMatrix._data[i, j] = _data[j, i];
            }
        }

        return transposedMatrix;
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++) sb.Append($"{_data[i, j]:F3} ");
            sb.AppendLine();
        }
        return sb.ToString();
    }
}