using WebApi.Dtos;
using WebApi.Models;
using WebApi.Requests;

namespace WebApi.Services;

public interface ILocationsService
{
    LocationDto Get(double latitude, double longitude);
    LocationDto Get(long pointId);
    LocationDto CreateLocation(LocationCreateRequest request);
    LocationDto UpdateLocation(long pointId, LocationCreateRequest request);
    void DeleteLocation(long pointId);
    string GetHashV1(LocationCords cords);
    string GetHashV2(LocationCords cords);
    string GetHashV3(LocationCords cords);
}