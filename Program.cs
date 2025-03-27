// See https://aka.ms/new-console-template for more information
//using Neo4jClient;
using Neo4j;

Neo4jDemo neo4J = new Neo4jDemo();

try
{
	await neo4J.ConnectToDB();

	Node mNode = new Node
	{
		Properties = new Dictionary<string, object>
				{
					{ "tagline", "Super start Noeo4j" },
					{ "title", "Neo4jTest123" },
					{ "released", 2025 },
					{ "Revenue", 202500 }
				}
	};
	Node pNode = new Node
	{
		Properties = new Dictionary<string, object>
				{
					{ "name", "SKY BPI" },
					{ "born", 1956 }
				}
	};
	// Create Movie and Person Node
	await neo4J.CreateMovieNode(mNode);
	//await neo4J.CreateMovieNode(mNode);
	//await neo4J.CreatePersonNode(pNode);

	// Create new Test Node
	//await neo4J.CreateTestNode(mNode);
	//await neo4J.DeleteTest("Neo4j");

	// Create Relationship
	//string relation = "[:ACTED_IN]";
	//await neo4J.CreateRelationship("SKY BPI", "Neo4j", relation);

	// Delete Movie and Person
	//await neo4J.DeleteMovie("Neo4j");
	//await neo4J.DeletePerson("SKY BPI");
}
catch (Exception ex)
{
	Console.WriteLine($"An error occurred: {ex.Message}");
}
finally
{
	neo4J.CloseConnection();
}
