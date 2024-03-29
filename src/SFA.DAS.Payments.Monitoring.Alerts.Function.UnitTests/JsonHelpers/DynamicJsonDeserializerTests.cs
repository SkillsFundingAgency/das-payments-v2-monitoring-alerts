using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Payments.Monitoring.Alerts.Function.JsonHelpers;
using System.Dynamic;
using System.Text.Json;

namespace SFA.DAS.Payments.Monitoring.Alerts.Function.UnitTests.JsonHelpers
{
    public class Tests
    {
        [Test]  
        public void NullArgumentReturnsNull()
        {
            //Arrange
            var deserializer = new DynamicJsonDeserializer();
            string? input = null;

            //Act
            var result = deserializer.Deserialize(input);
            var castedObject = (object)result;

            //Assert
            castedObject.Should().BeNull();
        }

        [Test]
        public void ReturnsExpandoObject()
        {
            //Arrange
            var deserializer = new DynamicJsonDeserializer();
            var input = "{\"property\": \"value\"}";

            //Act
            var result = deserializer.Deserialize(input);
            var castedObject = (object)result;

            //Assert
            castedObject.Should().BeOfType<ExpandoObject>();
        }

        [Test]
        public void SimpleObjectWithInvalidJsonThrowsInvalidJsonException()
        {
            //Arrange
            var deserializer = new DynamicJsonDeserializer();
            var input = "{property: \"\"}";

            //Act 
            System.Func<dynamic> act = () => deserializer.Deserialize(input);

            //Assert
            act.Should()
               .Throw<JsonException>()
               .WithMessage("'p' is an invalid start of a property name. Expected a '\"'. Path: $ | LineNumber: 0 | BytePositionInLine: 1.");
        }

        [Test]
        public void DeserializesSimpleObjectPropertyWhenTypeIsStringAndValueSetGetsType()
        {
            //Arrange
            var deserializer = new DynamicJsonDeserializer();

            var input = "{\"property\": \"value\"}";

            //Act
            var result = deserializer.Deserialize(input);
            var castedObject = (object)result.property;

            //Assert
            castedObject.Should().BeOfType<string>();
        }

        [Test]
        public void DeserializesSimpleObjectPropertyWhenTypeIsStringAndValueSetGetsValue()
        {
            //Arrange
            var deserializer = new DynamicJsonDeserializer();

            var input = "{\"property\": \"value\"}";

            //Act
            var result = deserializer.Deserialize(input);
            var castedObject = (string)result.property;

            //Assert
            castedObject.Should().Be("value");
        }

        [Test] 
        public void DeserializesSimpleObjectPropertyWhenTypeIsStringAndValueIsEmptyGetsType()
        {
            //Arrange
            var deserializer = new DynamicJsonDeserializer();
            var input = "{\"property\": \"\"}";

            //Act
            var result = deserializer.Deserialize(input);
            var castedObject = (object)result.property;

            //Assert
            castedObject.Should().BeOfType<string>();
        }

        [Test]
        public void DeserializesSimpleObjectPropertyWhenTypeIsStringAndValueIsEmptyGetsValue()
        {
            //Arrange
            var deserializer = new DynamicJsonDeserializer();

            var input = "{\"property\": \"\"}";

            //Act
            var result = deserializer.Deserialize(input);
            var castedObject = (string)result.property;

            //Assert
            castedObject.Should().Be(string.Empty);
        }

        [Test]
        public void DeserializesSimpleObjectPropertyWhenValueIsNullGetsType()
        {
            //Arrange
            var deserializer = new DynamicJsonDeserializer();

            var input = "{\"property\": null}";

            //Act
            var result = deserializer.Deserialize(input);
            var castedObject = (object)result.property;

            //Assert
            castedObject.Should().BeNull();
        }

        [Test]
        public void DeserializesSimpleObjectPropertyWhenTypeIsIntAndValueSetGetType()
        {
            //Arrange
            var deserializer = new DynamicJsonDeserializer();

            var input = "{\"property\": 3}";

            //Act
            var result = deserializer.Deserialize(input);
            var castedObject = (object)result.property;

            //Assert
            castedObject.Should().BeOfType<int>();
        }

        [Test]
        public void DeserializesSimpleObjectPropertyWhenTypeIsIntAndValueSetGetValue()
        {
            //Arrange
            var deserializer = new DynamicJsonDeserializer();

            var input = "{\"property\": 3}";

            //Act
            var result = deserializer.Deserialize(input);
            var castedObject = (object)result.property;

            //Assert
            castedObject.Should().Be(3);
        }

        [Test]
        public void DeserializesSimpleObjectPropertyWhenTypeIsInt64AndValueSetGetType()
        {
            //Arrange
            var deserializer = new DynamicJsonDeserializer();

            var input = "{\"property\": 2147483649}";

            //Act
            var result = deserializer.Deserialize(input);
            var castedObject = (object)result.property;

            //Assert
            castedObject.Should().BeOfType<long>();
        }

        [Test]
        public void DeserializesSimpleObjectPropertyWhenTypeIsInt64AndValueSetGetValue()
        {
            //Arrange
            var deserializer = new DynamicJsonDeserializer();

            var input = "{\"property\": 2147483649}";

            //Act
            var result = deserializer.Deserialize(input);
            var castedObject = (object)result.property;

            //Assert
            castedObject.Should().Be(2147483649);
        }

        [Test]
        public void DeserializesSimpleObjectPropertyWhenTypeIsDoubleAndValueSetGetType()
        {
            //Arrange
            var deserializer = new DynamicJsonDeserializer();

            var input = "{\"property\": 3.1415}";

            //Act
            var result = deserializer.Deserialize(input);
            var castedObject = (object)result.property;

            //Assert
            castedObject.Should().BeOfType<double>();
        }

        [Test]
        public void DeserializesSimpleObjectPropertyWhenTypeIsDoubleAndValueSetGetValue()
        {
            //Arrange
            var deserializer = new DynamicJsonDeserializer();

            var input = "{\"property\": 3.1415}";

            //Act
            var result = deserializer.Deserialize(input);
            var castedObject = (object)result.property;

            //Assert
            castedObject.Should().Be(3.1415);
        }

        [Test]
        public void DeserializesSimpleObjectPropertyWhenTypeIsNumericAndValueInvalidThrowsException()
        {
            //Arrange
            var deserializer = new DynamicJsonDeserializer();

            var input = "{\"property\": 3.1415.12}";

            //Act
            System.Func<dynamic> act = () => deserializer.Deserialize(input);

            //Assert
            act.Should()
                .Throw<JsonException>()
                .WithMessage("'.' is an invalid end of a number. Expected 'E' or 'e'. Path: $ | LineNumber: 0 | BytePositionInLine: 19.");
        }

        [Test]
        public void DeserializesSimpleObjectPropertyWhenTypeIsBooleanAndValueSetGetType()
        {
            //Arrange
            var deserializer = new DynamicJsonDeserializer();

            var input = "{\"property\": true}";

            //Act
            var result = deserializer.Deserialize(input);
            var castedObject = (object)result.property;

            //Assert
            castedObject.Should().BeOfType<bool>();
        }

        [Test]
        public void DeserializesSimpleObjectPropertyWhenTypeIsBooleanAndValueSetGetValue()
        {
            //Arrange
            var deserializer = new DynamicJsonDeserializer();

            var input = "{\"property\": true}";

            //Act
            var result = deserializer.Deserialize(input);
            var castedObject = (object)result.property;

            //Assert
            castedObject.Should().Be(true);
        }

        [Test]
        public void DeserializesSimpleObjectPropertyWhenTypeIsBooleanAndValueInvalidThrowsException()
        {
            //Arrange
            var deserializer = new DynamicJsonDeserializer();

            var input = "{\"property\": falsetrue}";

            //Act
            System.Func<dynamic> act = () => deserializer.Deserialize(input);

            //Assert
            act.Should().Throw<JsonException>();
        }

        [Test]
        public void DeserializezSimpleObjectPropertiesValueTest()
        {
            //Arrange 
            var deserializer = new DynamicJsonDeserializer();

            var input = @"
            {
	            ""textProperty"" : ""textValue"",
	            ""boolProperty"" : true,
	            ""intProperty"" : 42,
	            ""longProperty"": 2147483649,
	            ""doubleProperty"": 3.1415
            }";

            //Act
            var result = deserializer.Deserialize(input);

            //Assert
            var castedTextProperty = (object)result.textProperty;
            var castedBoolProperty = (object)result.boolProperty;
            var castedIntProperty = (object)result.intProperty;
            var castedLongProperty = (object)result.longProperty;
            var castedDoubleProperty = (object)result.doubleProperty;

            castedTextProperty.Should().BeOfType<string>().Which.Should().Be("textValue");
            castedBoolProperty.Should().BeOfType<bool>().Which.Should().Be(true);
            castedIntProperty.Should().BeOfType<int>().Which.Should().Be(42);
            castedLongProperty.Should().BeOfType<long>().Which.Should().Be(2147483649);
            castedDoubleProperty.Should().BeOfType<double>().Which.Should().Be(3.1415);
        }

        [Test]
        public void DeserializesSimpleNestedObjectAsExpandoObject()
        {
            //Arrange 
            var deserializer = new DynamicJsonDeserializer();

            var input = @"
            {
                ""nestedObject"": 
                {
                    ""property"": ""textValue""
                }
            }";

            //Act
            var result = deserializer.Deserialize(input);

            //Assert
            var castedNestedObject = (object)result.nestedObject;

            castedNestedObject.Should().BeOfType<ExpandoObject>();
        }

        [Test]
        public void DeserializesSimpleNestedObjectStringPropertyValueSetGetsType()
        {
            //Arrange 
            var deserializer = new DynamicJsonDeserializer();

            var input = @"
            {
                ""nestedObject"": 
                {
                    ""property"": ""textValue""
                }
            }";

            //Act
            var result = deserializer.Deserialize(input);

            //Assert
            var nestedObjectTextProperty = (object)result.nestedObject.property;

            nestedObjectTextProperty.Should().BeOfType<string>();
        }

        [Test]
        public void DeserializesSimpleNestedObjectStringPropertyValueSetGetsValue()
        {
            //Arrange 
            var deserializer = new DynamicJsonDeserializer();

            var input = @"
            {
                ""nestedObject"": 
                {
                    ""property"": ""textValue""
                }
            }";

            //Act
            var result = deserializer.Deserialize(input);

            //Assert
            var nestedObjectTextProperty = (object)result.nestedObject.property;

            nestedObjectTextProperty.Should().Be("textValue");
        }

        [Test]
        public void DeserializesSimpleNestedObjectNullPropertyValueEmptyGetsType()
        {
            //Arrange 
            var deserializer = new DynamicJsonDeserializer();

            var input = @"
            {
                ""nestedObject"": 
                {
                    ""property"": null
                }
            }";

            //Act
            var result = deserializer.Deserialize(input);

            //Assert
            var nestedObjectTextProperty = (object)result.nestedObject.property;

            nestedObjectTextProperty.Should().BeNull();
        }

        [Test]
        public void DeserializesSimpleNestedObjectStringPropertyValueEmptyGetsType()
        {
            //Arrange 
            var deserializer = new DynamicJsonDeserializer();

            var input = @"
            {
                ""nestedObject"": 
                {
                    ""property"": """"
                }
            }";

            //Act
            var result = deserializer.Deserialize(input);

            //Assert
            var nestedObjectTextProperty = (object)result.nestedObject.property;

            nestedObjectTextProperty.Should().BeOfType<string>();
        }

        [Test]
        public void DeserializesSimpleNestedObjectStringPropertyValueEmptyGetsValue()
        {
            //Arrange 
            var deserializer = new DynamicJsonDeserializer();

            var input = @"
            {
                ""nestedObject"": 
                {
                    ""property"": """"
                }
            }";

            //Act
            var result = deserializer.Deserialize(input);

            //Assert
            var nestedObjectTextProperty = (object)result.nestedObject.property;
            nestedObjectTextProperty.Should().Be(string.Empty);
        }

        [Test]
        public void DeserializesSimpleNestedObjectIntPropertyGetsType()
        {
            //Arrange 
            var deserializer = new DynamicJsonDeserializer();

            var input = @"
            {
                ""nestedObject"": 
                {
                    ""property"": 42
                }
            }";

            //Act
            var result = deserializer.Deserialize(input);

            //Assert
            var nestedObjectTextProperty = (object)result.nestedObject.property;
            nestedObjectTextProperty.Should().BeOfType<int>();
        }

        [Test]
        public void DeserializesSimpleNestedObjectIntPropertyGetsValue()
        {
            //Arrange 
            var deserializer = new DynamicJsonDeserializer();

            var input = @"
            {
                ""nestedObject"": 
                {
                    ""property"": 42
                }
            }";

            //Act
            var result = deserializer.Deserialize(input);

            //Assert
            var nestedObjectTextProperty = (object)result.nestedObject.property;
            nestedObjectTextProperty.Should().Be(42);
        }

        [Test]
        public void DeserializesSimpleNestedObjectDoublePropertyGetsType()
        {
            //Arrange 
            var deserializer = new DynamicJsonDeserializer();

            var input = @"
            {
                ""nestedObject"": 
                {
                    ""property"": 3.1415
                }
            }";

            //Act
            var result = deserializer.Deserialize(input);

            //Assert
            var nestedObjectTextProperty = (object)result.nestedObject.property;
            nestedObjectTextProperty.Should().BeOfType<double>();
        }

        [Test]
        public void DeserializesSimpleNestedObjectDoublePropertyGetsValue()
        {
            //Arrange 
            var deserializer = new DynamicJsonDeserializer();

            var input = @"
            {
                ""nestedObject"": 
                {
                    ""property"": 3.1415
                }
            }";

            //Act
            var result = deserializer.Deserialize(input);

            //Assert
            var nestedObjectTextProperty = (object)result.nestedObject.property;
            nestedObjectTextProperty.Should().Be(3.1415);
        }

        public void DeserializesSimpleNestedObjectBoolPropertyGetsType()
        {
            //Arrange 
            var deserializer = new DynamicJsonDeserializer();

            var input = @"
            {
                ""nestedObject"": 
                {
                    ""property"": true
                }
            }";

            //Act
            var result = deserializer.Deserialize(input);

            //Assert
            var nestedObjectTextProperty = (object)result.nestedObject.property;
            nestedObjectTextProperty.Should().BeOfType<bool>();
        }

        [Test]
        public void DeserializesSimpleNestedObjectBoolPropertyGetsValue()
        {
            //Arrange 
            var deserializer = new DynamicJsonDeserializer();

            var input = @"
            {
                ""nestedObject"": 
                {
                    ""property"": true
                }
            }";

            //Act
            var result = deserializer.Deserialize(input);

            //Assert
            var nestedObjectTextProperty = (object)result.nestedObject.property;
            nestedObjectTextProperty.Should().Be(true);
        }
    }
}