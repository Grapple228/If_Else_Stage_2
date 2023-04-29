using Database.Models;

namespace WebApi.Models;

public struct Segment
{
    public Segment(ILocation start, ILocation end)
    {
        Start = start;
        End = end;
    }

    public readonly ILocation Start;
    public readonly ILocation End;
}