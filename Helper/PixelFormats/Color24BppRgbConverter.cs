using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;

namespace Helper.PixelFormats
{
    /// <summary>
    /// Converts colors from one data type to another. Access this class through the
    /// <see cref="TypeDescriptor"/>.
    /// </summary>
    public class Color24BppRgbConverter : TypeConverter
    {
        /// <inheritdoc/>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;
            return base.CanConvertFrom(context, sourceType);
        }

        /// <inheritdoc/>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(InstanceDescriptor))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        /// <inheritdoc/>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var text = value as string;
            if (text != null)
            {
                text = text.Trim();

                if (text.Length == 0)
                    return null;

                if (culture == null)
                    culture = SR.CurrentCulture;

                var sep = culture.TextInfo.ListSeparator[0];
                var tokens = text.Split(new char[] { sep });
                var values = new int[tokens.Length];
                var intConverter = TypeDescriptor.GetConverter(typeof(int));
                for (int i = 0; i < values.Length; i++)
                    values[i] = (int)intConverter.ConvertFromString(context, culture, tokens[i]);

                if (values.Length == 3)
                    return new Color24BppRgb(values[0], values[1], values[2]);
                else
                    throw new ArgumentException(nameof(text));
            }
            return base.ConvertFrom(context, culture, value);
        }

        /// <inheritdoc/>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == null)
                throw new ArgumentNullException(nameof(destinationType));

            if (value is Color24BppRgb)
            {
                if (destinationType == typeof(string))
                {
                    var color = (Color24BppRgb)value;

                    if (culture == null)
                        culture = SR.CurrentCulture;

                    var sep = culture.TextInfo.ListSeparator + " ";
                    var intConverter = TypeDescriptor.GetConverter(typeof(int));
                    var args = new string[3];
                    var nArg = 0;

                    args[nArg++] = intConverter.ConvertToString(context, culture, color.Red);
                    args[nArg++] = intConverter.ConvertToString(context, culture, color.Green);
                    args[nArg++] = intConverter.ConvertToString(context, culture, color.Blue);

                    return String.Join(sep, args);
                }
                if (destinationType == typeof(InstanceDescriptor))
                {
                    var color = (Color24BppRgb)value;

                    var ctor = typeof(Color24BppRgb).GetConstructor(new Type[] { typeof(int), typeof(int), typeof(int) });
                    if (ctor != null)
                        return new InstanceDescriptor(ctor, new object[] { color.Red, color.Green, color.Blue });
                }
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        /// <inheritdoc/>
        public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
        {
            if (propertyValues == null)
                throw new ArgumentNullException(nameof(propertyValues));

            var red = propertyValues[nameof(Color24BppRgb.Red)];
            var green = propertyValues[nameof(Color24BppRgb.Green)];
            var blue = propertyValues[nameof(Color24BppRgb.Blue)];

            if (red == null || !(red is int))
                throw new ArgumentException(nameof(Color24BppRgb.Red));
            if (green == null || !(green is int))
                throw new ArgumentException(nameof(Color24BppRgb.Green));
            if ((blue == null) || !(blue is int))
                throw new ArgumentException(nameof(Color24BppRgb.Blue));

            return new Color24BppRgb((int)red, (int)green, (int)blue);
        }

        /// <inheritdoc/>
        public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        /// <inheritdoc/>
        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            var props = TypeDescriptor.GetProperties(typeof(Color24BppRgb), attributes);
            return props.Sort(new string[] {
                nameof(Color24BppRgb.Red),
                nameof(Color24BppRgb.Green),
                nameof(Color24BppRgb.Blue) });
        }

        /// <inheritdoc/>
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
    }
}
