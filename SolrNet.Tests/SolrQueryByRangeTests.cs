﻿#region license
// Copyright (c) 2007-2010 Mauricio Scheffer
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//      http://www.apache.org/licenses/LICENSE-2.0
//  
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

using System;
using Xunit;
using SolrNet.Impl.FieldSerializers;
using SolrNet.Impl.QuerySerializers;

namespace SolrNet.Tests
{

    public class SolrQueryByRangeTests
    {
        public class TestDocument { }

        public string Serialize(object q)
        {
            var serializer = new DefaultQuerySerializer(new DefaultFieldSerializer());
            return serializer.Serialize(q);
        }


        [Fact]
        public void IntInclusive()
        {
            var q = new SolrQueryByRange<int>("id", 123, 456);
            Assert.Equal("id:[123 TO 456]", Serialize(q));
        }

        [Fact]
        public void StringInclusive()
        {
            var q = new SolrQueryByRange<string>("id", "Arroz", "Zimbabwe");
            Assert.Equal("id:[Arroz TO Zimbabwe]", Serialize(q));
        }

        [Fact]
        public void StringExclusive()
        {
            var q = new SolrQueryByRange<string>("id", "Arroz", "Zimbabwe", false);
            Assert.Equal("id:{Arroz TO Zimbabwe}", Serialize(q));
        }

        [Fact]
        public void FloatInclusive()
        {
            var q = new SolrQueryByRange<float>("price", 123.45f, 234.56f);
            Assert.Equal("price:[123.45 TO 234.56]", Serialize(q));
        }

        [Fact]
        public void NullableFloat()
        {
            var q = new SolrQueryByRange<float?>("price", null, 234.56f);
            Assert.Equal("price:[* TO 234.56]", Serialize(q));
        }

        [Fact]
        public void DateTimeInclusive()
        {
            var q = new SolrQueryByRange<DateTime>("ts", new DateTime(2001, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(2002, 3, 4, 5, 6, 7, DateTimeKind.Utc));
            Assert.Equal("ts:[2001-01-05T00:00:00Z TO 2002-03-04T05:06:07Z]", Serialize(q));
        }

        [Fact]
        public void NullableDateTime()
        {
            var q = new SolrQueryByRange<DateTime?>("ts", new DateTime(2001, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(2002, 3, 4, 5, 6, 7,  DateTimeKind.Utc));
            Assert.Equal("ts:[2001-01-05T00:00:00Z TO 2002-03-04T05:06:07Z]", Serialize(q));
        }

        [Fact]
        public void NullableDateTimeWithNull()
        {
            var q = new SolrQueryByRange<DateTime?>("ts", null, new DateTime(2002, 3, 4, 5, 6, 7, DateTimeKind.Utc));
            Assert.Equal("ts:[* TO 2002-03-04T05:06:07Z]", Serialize(q));
        }


        [Fact]
        public void IntLowerInclusiveUpperInclusive()
        {
            var q = new SolrQueryByRange<int>("id", 123, 456, true, true);
            Assert.Equal("id:[123 TO 456]", Serialize(q));
        }

        [Fact]
        public void IntLowerInclusiveUpperExclusive()
        {
            var q = new SolrQueryByRange<int>("id", 123, 456, true, false);
            Assert.Equal("id:[123 TO 456}", Serialize(q));
        }

        [Fact]
        public void IntLowerExclusiveUpperInclusive()
        {
            var q = new SolrQueryByRange<int>("id", 123, 456, false, true);
            Assert.Equal("id:{123 TO 456]", Serialize(q));
        }

        [Fact]
        public void IntLowerExclusiveUpperExclusive()
        {
            var q = new SolrQueryByRange<int>("id", 123, 456, false, false);
            Assert.Equal("id:{123 TO 456}", Serialize(q));
        }


        [Fact]
        public void IntLowerExclusiveUpperInclusiveWithNull()
        {
            var q = new SolrQueryByRange<int?>("id", 123, null, false, true);
            Assert.Equal("id:{123 TO *]", Serialize(q));
        }

        [Fact]
        public void IntLowerExclusiveUpperExclusiveWithNull()
        {
            var q = new SolrQueryByRange<int?>("id", 123, null, false, false);
            Assert.Equal("id:{123 TO *}", Serialize(q));
        }
    }
}
