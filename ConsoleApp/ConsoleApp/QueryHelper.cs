using ConsoleApp.Model;
using ConsoleApp.Model.Enum;
using ConsoleApp.OutputTypes;

namespace ConsoleApp;

public class QueryHelper : IQueryHelper
{
    /// <summary>
    /// Get Deliveries that has payed
    /// </summary>
    public IEnumerable<Delivery> Paid(IEnumerable<Delivery> deliveries) => 
    //TODO: Завдання 1
    deliveries.Where(d => d.PaymentId != null);
    /// <summary>
    /// Get Deliveries that now processing by system (not Canceled or Done)
    /// </summary>
    public IEnumerable<Delivery> NotFinished(IEnumerable<Delivery> deliveries) =>
    //TODO: Завдання 2 
    deliveries.Where(d => d.Status != DeliveryStatus.Canceled && d.Status != DeliveryStatus.Completed);
    /// <summary>
    /// Get DeliveriesShortInfo from deliveries of specified client
    /// </summary>
    public IEnumerable<DeliveryShortInfo> DeliveryInfosByClient(IEnumerable<Delivery> deliveries, string clientId) =>
    //TODO: Завдання 3
    deliveries
        .Where(d => d.ClientId == clientId)
        .Select(d => new DeliveryShortInfo
        {
            DeliveryId = d.Id,
            Status = d.Status,
            StartLocation = d.Direction.Origin,
            EndLocation = d.Direction.Destination
        });
    /// <summary>
    /// Get first ten Deliveries that starts at specified city and have specified type
    /// </summary>
    public IEnumerable<Delivery> DeliveriesByCityAndType(IEnumerable<Delivery> deliveries, string cityName, DeliveryType type) => 
    //TODO: Завдання 4
    deliveries
        .Where(d => d.Direction.Origin.City == cityName && d.Type == type)
        .Take(10);
    /// <summary>
    /// Order deliveries by status, then by start of loading period
    /// </summary>
    public IEnumerable<Delivery> OrderByStatusThenByStartLoading(IEnumerable<Delivery> deliveries) => 
    //TODO: Завдання 5
    deliveries
        .OrderBy(d => d.Status)
        .ThenBy(d => d.LoadingPeriod.Start);
    /// <summary>
    /// Count unique cargo types
    /// </summary>
    public int CountUniqCargoTypes(IEnumerable<Delivery> deliveries) => 
    //TODO: Завдання 6
    deliveries
        .Select(d=> d.CargoType)
        .Distinct()
        .Count();
    /// <summary>
    /// Group deliveries by status and count deliveries in each group
    /// </summary>
    public Dictionary<DeliveryStatus, int> CountsByDeliveryStatus(IEnumerable<Delivery> deliveries) => 
    //TODO: Завдання 7
    deliveries
        .GroupBy(d=> d.Status)
        .ToDictionary(g => g.Key, g.Count());
    /// <summary>
    /// Group deliveries by start-end city pairs and calculate average gap between end of loading period and start of arrival period (calculate in minutes)
    /// </summary>
    public IEnumerable<AverageGapsInfo> AverageTravelTimePerDirection(IEnumerable<Delivery> deliveries) => 
    //TODO: Завдання 8
    deliveries
        .GroupBy(d => new {d.Direction.Origin, d.Direction.Destination})
        .Select(g => new AverageGapsInfo
        {
            StartLocation = g.Key.Origin,
            EndLocation= g.Key.Destination,
            AverageTime = g.Average(d=>(d.ArrivalPeriod.Start - d.LoadingPeriod.End).Value.TotalMinutes)
        });
    /// <summary>
    /// Paging helper
    /// </summary>
    public IEnumerable<TElement> Paging<TElement, TOrderingKey>(IEnumerable<TElement> elements,
        Func<TElement, TOrderingKey> ordering,
        Func<TElement, bool>? filter = null,
        int countOnPage = 100,
        int pageNumber = 1) => 
        //TODO: Завдання 9 
        elements
            .Where(filter ?? (e=>true))
            .OrderBy(ordering)
            .Skip((pageNumber - 1)* countOnPage)
            .Take(countOnPage)
}
