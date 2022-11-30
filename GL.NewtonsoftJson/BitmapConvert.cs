using Newtonsoft.Json;
using System;
using System.Drawing;

namespace GL.NewtonsoftJson
{
    public class BitmapConvert : JsonConverter
    {
        static Type BitmapType = typeof(Bitmap);

        public override bool CanConvert(Type objectType)
        {
            return objectType == BitmapType;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return null;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            Bitmap bmp = (Bitmap)value;

            writer.WriteValue(ImageUtils.BitmapToBase64String(bmp));
        }
    }
}
