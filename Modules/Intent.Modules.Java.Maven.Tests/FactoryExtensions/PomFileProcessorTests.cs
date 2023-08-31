using Intent.Modules.Common.Java.Templates;
using Intent.Modules.Java.Maven.FactoryExtensions;
using Shouldly;

namespace Intent.Modules.Java.Maven.Tests.FactoryExtensions
{
    public class PomFileProcessorTests
    {
        public class DescribeProcess
        {
            [Fact]
            public void ItShouldAddScope()
            {
                // Arrange
                var javaDependency = new JavaDependency(
                    groupId: "myGroup",
                    artifactId: "artifactId",
                    version: default,
                    type: default,
                    scope: JavaDependencyScope.Provided,
                    optional: default);

                var content = @"<?xml version=""1.0"" encoding=""utf-8""?>
<project xmlns=""http://maven.apache.org/POM/4.0.0"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xsi:schemaLocation=""http://maven.apache.org/POM/4.0.0 https://maven.apache.org/xsd/maven-4.0.0.xsd"">

	<dependencies>
		<dependency>
			<groupId>myGroup</groupId>
			<artifactId>artifactId</artifactId>
		</dependency>
	</dependencies>

</project>".ReplaceLineEndings();

                // Act
                var result = PomFileProcessor.Process(content, null, new[] { javaDependency });

                // Assert
                result.ShouldBe(@"<?xml version=""1.0"" encoding=""utf-8""?>
<project xmlns=""http://maven.apache.org/POM/4.0.0"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xsi:schemaLocation=""http://maven.apache.org/POM/4.0.0 https://maven.apache.org/xsd/maven-4.0.0.xsd"">

	<dependencies>
		<dependency>
			<groupId>myGroup</groupId>
			<artifactId>artifactId</artifactId>
			<scope>provided</scope>
		</dependency>
	</dependencies>

</project>".ReplaceLineEndings());
            }

            [Fact]
            public void ItShouldNotChangeScope()
            {
                // Arrange
                var javaDependency = new JavaDependency(
                    groupId: "myGroup",
                    artifactId: "artifactId",
                    version: default,
                    type: default,
                    scope: null,
                    optional: default);

                var content = @"<?xml version=""1.0"" encoding=""utf-8""?>
<project xmlns=""http://maven.apache.org/POM/4.0.0"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xsi:schemaLocation=""http://maven.apache.org/POM/4.0.0 https://maven.apache.org/xsd/maven-4.0.0.xsd"">

	<dependencies>
		<dependency>
			<groupId>myGroup</groupId>
			<artifactId>artifactId</artifactId>
			<scope>test</scope>
		</dependency>
	</dependencies>

</project>".ReplaceLineEndings();

                // Act
                var result = PomFileProcessor.Process(content, null, new[] { javaDependency });

                // Assert
                result.ShouldBe(@"<?xml version=""1.0"" encoding=""utf-8""?>
<project xmlns=""http://maven.apache.org/POM/4.0.0"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xsi:schemaLocation=""http://maven.apache.org/POM/4.0.0 https://maven.apache.org/xsd/maven-4.0.0.xsd"">

	<dependencies>
		<dependency>
			<groupId>myGroup</groupId>
			<artifactId>artifactId</artifactId>
			<scope>test</scope>
		</dependency>
	</dependencies>

</project>".ReplaceLineEndings());
            }

            [Fact]
            public void ItShouldAddType()
            {
                // Arrange
                var javaDependency = new JavaDependency(
                    groupId: "myGroup",
                    artifactId: "artifactId",
                    version: default,
                    type: "test-jar",
                    scope: default,
                    optional: default);

                var content = @"<?xml version=""1.0"" encoding=""utf-8""?>
<project xmlns=""http://maven.apache.org/POM/4.0.0"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xsi:schemaLocation=""http://maven.apache.org/POM/4.0.0 https://maven.apache.org/xsd/maven-4.0.0.xsd"">

	<dependencies>
		<dependency>
			<groupId>myGroup</groupId>
			<artifactId>artifactId</artifactId>
		</dependency>
	</dependencies>

</project>".ReplaceLineEndings();

                // Act
                var result = PomFileProcessor.Process(content, null, new[] { javaDependency });

                // Assert
                result.ShouldBe(@"<?xml version=""1.0"" encoding=""utf-8""?>
<project xmlns=""http://maven.apache.org/POM/4.0.0"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xsi:schemaLocation=""http://maven.apache.org/POM/4.0.0 https://maven.apache.org/xsd/maven-4.0.0.xsd"">

	<dependencies>
		<dependency>
			<groupId>myGroup</groupId>
			<artifactId>artifactId</artifactId>
			<type>test-jar</type>
		</dependency>
	</dependencies>

</project>".ReplaceLineEndings());
            }

            [Fact]
            public void ItShouldNotChangeType()
            {
                // Arrange
                var javaDependency = new JavaDependency(
                    groupId: "myGroup",
                    artifactId: "artifactId",
                    version: default,
                    type: default,
                    scope: null,
                    optional: default);

                var content = @"<?xml version=""1.0"" encoding=""utf-8""?>
<project xmlns=""http://maven.apache.org/POM/4.0.0"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xsi:schemaLocation=""http://maven.apache.org/POM/4.0.0 https://maven.apache.org/xsd/maven-4.0.0.xsd"">

	<dependencies>
		<dependency>
			<groupId>myGroup</groupId>
			<artifactId>artifactId</artifactId>
			<type>test-jar</type>
		</dependency>
	</dependencies>

</project>".ReplaceLineEndings();

                // Act
                var result = PomFileProcessor.Process(content, null, new[] { javaDependency });

                // Assert
                result.ShouldBe(@"<?xml version=""1.0"" encoding=""utf-8""?>
<project xmlns=""http://maven.apache.org/POM/4.0.0"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xsi:schemaLocation=""http://maven.apache.org/POM/4.0.0 https://maven.apache.org/xsd/maven-4.0.0.xsd"">

	<dependencies>
		<dependency>
			<groupId>myGroup</groupId>
			<artifactId>artifactId</artifactId>
			<type>test-jar</type>
		</dependency>
	</dependencies>

</project>".ReplaceLineEndings());
            }

            [Fact]
            public void ItShouldAddExclusion()
            {
                // Arrange
                var javaDependency = new JavaDependency(
                    groupId: "myGroup",
                    artifactId: "artifactId",
                    version: default,
                    exclusions: new List<JavaDependencyExclusion>
                    {
                        new("group1","artifact1")
                    },
                    type: default,
                    scope: null,
                    optional: default);

                var content = @"<?xml version=""1.0"" encoding=""utf-8""?>
<project xmlns=""http://maven.apache.org/POM/4.0.0"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xsi:schemaLocation=""http://maven.apache.org/POM/4.0.0 https://maven.apache.org/xsd/maven-4.0.0.xsd"">

	<dependencies>
		<dependency>
			<groupId>myGroup</groupId>
			<artifactId>artifactId</artifactId>
		</dependency>
	</dependencies>

</project>".ReplaceLineEndings();

                // Act
                var result = PomFileProcessor.Process(content, null, new[] { javaDependency });

                // Assert
                result.ShouldBe(@"<?xml version=""1.0"" encoding=""utf-8""?>
<project xmlns=""http://maven.apache.org/POM/4.0.0"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xsi:schemaLocation=""http://maven.apache.org/POM/4.0.0 https://maven.apache.org/xsd/maven-4.0.0.xsd"">

	<dependencies>
		<dependency>
			<groupId>myGroup</groupId>
			<artifactId>artifactId</artifactId>
			<exclusions>
				<exclusion>
					<groupId>group1</groupId>
					<artifactId>artifact1</artifactId>
				</exclusion>
			</exclusions>
		</dependency>
	</dependencies>

</project>".ReplaceLineEndings());
            }
        }
    }
}