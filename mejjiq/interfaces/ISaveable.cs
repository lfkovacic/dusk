using System;
using System.Collections.Generic;
using System.Text.Json.Nodes;

namespace dusk.mejjiq.interfaces
{
    public interface ISaveable
    {
        // Returns a serialized representation of the object (e.g., JSON structure)
        public JsonNode Serialize();

        // Rebuilds the object from the provided serialized representation
        public static abstract ISaveable Deserialize(JsonNode serializedData);
    }
}
