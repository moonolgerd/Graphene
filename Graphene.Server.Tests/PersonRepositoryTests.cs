using Graphene.Server.Models;
using NSubstitute;
using Redis.OM.Contracts;
using Redis.OM.Searching;

namespace Graphene.Server.Tests;

[TestFixture]
public class PersonRepositoryTests
{
    private IPersonRepository _personRepository;
    private IRedisCollection<Person> _redisCollectionMock;
    private IRedisConnectionProvider _connectionProviderMock;

    [SetUp]
    public void SetUp()
    {
        _redisCollectionMock = Substitute.For<IRedisCollection<Person>>();
        _connectionProviderMock = Substitute.For<IRedisConnectionProvider>();
        _connectionProviderMock.RedisCollection<Person>().Returns(_redisCollectionMock);
        _personRepository = new PersonRepository(_connectionProviderMock);
    }

    [Test]
    public async Task GetAllPeople_ReturnsListOfPeople()
    {
        var peopleList = new List<Person> { new Person { Id = "person1" }, new Person { Id = "person2" } };

        _redisCollectionMock.ToListAsync().Returns(peopleList);

        var result = await _personRepository.GetAllPeople();

        Assert.That(result, Is.EqualTo(peopleList));
    }

    [Test]
    public async Task GetPersonById_ReturnsPerson()
    {
        var person = new Person { Id = "person1", FirstName = "John Doe" };

        _redisCollectionMock.FindByIdAsync(person.Id).Returns(person);

        var result = await _personRepository.GetPersonById(person.Id);

        Assert.That(result, Is.EqualTo(person));
    }

    [Test]
    public async Task AddPerson_AddsNewPerson()
    {
        var person = new Person { Id = "person1", FirstName = "John Doe" };

        await _personRepository.AddPerson(person);

        await _redisCollectionMock.Received()
            .InsertAsync(Arg.Is<Person>(
                p => p.Id == person.Id &&
                p.FirstName == person.FirstName &&
                p.LastName == person.LastName));
    }

    [Test]
    public async Task UpdatePerson_UpdatesPerson()
    {
        var updatedPerson = new Person { Id = "person1", FirstName = "Jane Doe", Age = 40 };

        await _personRepository.UpdatePerson(updatedPerson);

        await _redisCollectionMock.Received()
            .UpdateAsync(Arg.Is<Person>(
                p => p.Id == updatedPerson.Id &&
                p.FirstName == updatedPerson.FirstName &&
                p.LastName == updatedPerson.LastName &&
                p.Age == updatedPerson.Age));
    }

    [Test]
    public async Task DeletePerson_DeletesPerson()
    {
        var expected = new Person { Id = "person1", FirstName = "John Doe" };

        _redisCollectionMock.FindByIdAsync(expected.Id).Returns(expected);

        await _personRepository.DeletePerson(expected);

        await _redisCollectionMock.Received().DeleteAsync(expected);
    }
}