namespace Tudormobile.AlphaVantage.Entities;

/// <summary>
/// Defines a contract for entities that can be used as a constraint in generic type parameters.
/// </summary>
/// <remarks>Implement this interface to indicate that a type represents an entity within the domain model. This
/// interface does not specify any members and serves as a marker for type constraints.</remarks>
public interface IEntity { }    // for constraint on AlphaVantageResponse (at least for now)
