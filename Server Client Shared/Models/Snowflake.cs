namespace Shared.Models
{
    using Newtonsoft.Json;

    public class Snowflake
    {
        [JsonProperty("Id")]
        public long Id { get; set; }

        protected bool Equals(Snowflake other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object target)
        {
            if (target is null)
            {
                return false;
            }

            if (ReferenceEquals(this, target))
            {
                return true;
            }

            if (target.GetType() != GetType())
            {
                return false;
            }

            return Equals((Snowflake)target);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public static bool operator ==(Snowflake source, Snowflake target)
        {
            if (source is null && target is null)
            {
                return true;
            }

            if (source is null || target is null)
            {
                return false;
            }

            return source.Id == target.Id;
        }

        public static bool operator !=(Snowflake source, Snowflake target)
        {
            return !(source == target);
        }
    }
}