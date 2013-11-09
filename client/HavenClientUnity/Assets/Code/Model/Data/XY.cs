using System;
using System.Collections;

public class XY : IJsonable {
    public int X { get; private set; }
    public int Y { get; private set; }

    public XY(int x, int y) {
        X = x;
        Y = y;
    }

    public XY(Hashtable json) {
        FromJson(json);
    }

    public void FromJson(Hashtable json) {
        X = int.Parse(json["X"].ToString());
        Y = int.Parse(json["Y"].ToString());
    }

    public Hashtable ToJson() {
        Hashtable json = new Hashtable();

        json["X"] = X;
        json["Y"] = Y;

        return json;
    }

    public override string ToString() {
        return string.Format("XY ({0}, {1})", X, Y);
    }

    public static XY operator +(XY a, XY b) {
        return new XY(a.X + b.X, a.Y + b.Y);
    }

    public static XY operator -(XY a, XY b) {
        return new XY(a.X - b.X, a.Y - b.Y);
    }

    public static XY operator *(XY a, int b) {
        return new XY(a.X * b, a.Y * b);
    }

    public static XY operator /(XY a, int b) {
        return new XY(a.X / b, a.Y / b);
    }

    public override bool Equals(Object other) {
        if(other == null) return false;

        XY otherXY = other as XY;
        if(otherXY == null) return false;

        return (X == otherXY.X) && (Y == otherXY.Y);
    }

    public bool Equals(XY other) {
        if(other == null) return false;
        return (X == other.X) && (Y == other.Y);
    }

    public override int GetHashCode() {
        return X ^ Y;
    }
}
