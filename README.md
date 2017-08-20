# FluentComparator

## Synopsis

FluendComparator is a .NET library which allows you compare generic objects effectively. 
It makes it easy to exclude certain properties from the comparison process and can return the list of differences. 
The library was built with the Fluent interface in mind as the name suggests. It uses reflection to retrive the public properties and serializes them before comparison. 

## Code Example

```csharp
Comparator.Create<T>()
  .Compare(objectA)
  .To(objectB)
  .ExcludeProperty(x => x.SomeProperty)
  .EnableDifferences()
  .Evaluate();
```

* **Compare** and **To** methods will set the comparable objects
* **ExcludeProperty** can be called to ignore certain properties in the comparison process
* **EnableDifferences** and **DisableDifferences** can be called to either include or exclude the found differences in the returned result
* **Evaluate** method will execute the comparison and will return with a [ComparisonResult](https://github.com/petertasner/FluentComparator/blob/master/FluentComparator/Models/ComparisonResult.cs) object.
