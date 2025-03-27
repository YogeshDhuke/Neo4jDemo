using Neo4jClient;

namespace Neo4j
{
	public class Neo4jDemo
	{
		private GraphClient _client = null;

		public async Task ConnectToDB()
		{
			Uri uri = new Uri("http://localhost:7474");
			string user = "neo4j";
			string password = "Neo4j@1234";

			try
			{
				_client = new GraphClient(uri, user, password);
				await _client.ConnectAsync();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred: {ex.Message}");
			}
		}

		public void CloseConnection()
		{
			_client.Dispose();
		}

		public async Task CreatePersonNode(Node node)
		{
			if (node != null && node.Properties != null)
			{
				try
				{
					var query = _client.Cypher
								.Create("(p:Person $props)")
								.WithParam("props", node.Properties);
					await query.ExecuteWithoutResultsAsync();
				}
				catch (Exception ex)
				{
					Console.WriteLine($"An error occurred: {ex.Message}");
				}
			}
		}

		public async Task CreateMovieNode(Node node)
		{
			if (node != null && node.Properties != null)
			{
				try
				{
					var query = _client.Cypher
								.Create("(m:Movie $props)")
								.WithParam("props", node.Properties);
					await query.ExecuteWithoutResultsAsync();
				}
				catch (Exception ex)
				{
					Console.WriteLine($"An error occurred: {ex.Message}");
				}
			}
		}

		public async Task CreateTestNode(Node node)
		{
			if (node != null && node.Properties != null)
			{
				try
				{
					var query = _client.Cypher
								.Create("(t:Test $props)")
								.WithParam("props", node.Properties);
					await query.ExecuteWithoutResultsAsync();
				}
				catch (Exception ex)
				{
					Console.WriteLine($"An error occurred: {ex.Message}");
				}
			}
		}

		public async Task CreateRelationship(string pName, string mTitle, string relation)
		{
			try
			{
				var query = _client.Cypher
							.Match("(p:Person {name: $pName}), (m:Movie {title: $mTitle})")
							.WithParams(new
							{
								pName,
								mTitle
							})
							.Create($"(p)-{relation}->(m)");
				await query.ExecuteWithoutResultsAsync();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred: {ex.Message}");
			}
		}

		public async Task DeleteMovie(string movieTitle)
		{
			await _client.Cypher
				.Match("(m:Movie {title: $movieTitle})")
				.WithParam("movieTitle", movieTitle)
				.DetachDelete("m")
				.ExecuteWithoutResultsAsync();
		}

		public async Task DeletePerson(string pName)
		{
			await _client.Cypher
				.Match("(p:Person {name: $pName})")
				.WithParam("pName", pName)
				.DetachDelete("p")
				.ExecuteWithoutResultsAsync();
		}

		public async Task DeleteTest(string movieTitle)
		{
			await _client.Cypher
				.Match("(m:Test {title: $movieTitle})")
				.WithParam("movieTitle", movieTitle)
				.DetachDelete("m")
				.ExecuteWithoutResultsAsync();
		}


		//public async Task CreateNodeWithRelationship()
		//{
		//	var node = new Node
		//	{
		//		Properties = new Dictionary<string, object>
		//		{
		//			{ "name", "Gene Hackman" },
		//			{ "born", 1930 }
		//		}
		//	};
		//	try
		//	{
		//		var query = _client.Cypher
		//					.Match("(m:Movie)")
		//					.Where((Movie m) => m.title == "The Replacements")
		//					.Create("(p:Person {props})-[:DIRECTED]->(m)")
		//					.WithParam("props", node.Properties);
		//		await query.ExecuteWithoutResultsAsync();
		//	}
		//	catch (Exception ex)
		//	{
		//		Console.WriteLine($"An error occurred: {ex.Message}");
		//	}
		//}

		//public async Task CreateNodeWithMultipleRelationships()
		//{
		//	var node = new Node
		//	{
		//		Properties = new Dictionary<string, object>
		//		{
		//			{ "name", "Keanu Reeves" },
		//			{ "born", 1964 }
		//		}
		//	};
		//	try
		//	{
		//		var query = _client.Cypher
		//					.Match("(m:Movie)")
		//					.Where((Movie m) => m.title == "The Replacements")
		//					.Create("(p:Person {props})-[:ACTED_IN]->(m)")
		//					.Create("(p)-[:ACTED_IN]->(:Movie{title: 'Speed'})")
		//					.WithParam("props", node.Properties);
		//		await query.ExecuteWithoutResultsAsync();
		//	}
		//	catch (Exception ex)
		//	{
		//		Console.WriteLine($"An error occurred: {ex.Message}");
		//	}
		//}

		public async Task GetActors()
		{
			try
			{
				var query = _client.Cypher
							.Match("(:Person)-[:DIRECTED]->(:Movie{title: 'The Replacements'})<-[:ACTED_IN]-(n2:Person)")
							.Return((n2) => new
							{
								//Director = n1.As<Person>(),
								//Movie = m.As<Movie>(),
								Actor = n2.As<Person>()
							});

				var result = await query.ResultsAsync;

				foreach (var item in result)
				{
					Console.WriteLine($"{item.Actor.name} is actor in a movie The Replacements.");
					//Console.WriteLine($"{item.Movie.title} is Directed by {item.Director.name} and {item.Actor.name} is actor.");
					//Console.WriteLine("Director Properties:");
					//Console.WriteLine($"Name: {item.Director.name}");

					//Console.WriteLine("Movie Properties:");
					//Console.WriteLine($"Title: {item.Movie.title}, Tagline: {item.Movie.tagline}");

					//Console.WriteLine("Actor Properties:");
					//Console.WriteLine($"Name: {item.Actor.name}/n");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred: {ex.Message}");
			}
		}
	}

	public class Node
	{
		public IDictionary<string, object> Properties { get; set; }

		public override string ToString()
		{
			return string.Join(", ", Properties.Select(kv => $"{kv.Key}: {kv.Value}"));
		}
	}

	public class Person
	{
		public int born { get; set; }
		public string name { get; set; }
	}

	public class Movie
	{
		public string tagline { get; set; }
		public string title { get; set; }
		public int released { get; set; }
	}
}
