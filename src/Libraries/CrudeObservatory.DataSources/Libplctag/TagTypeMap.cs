using CrudeObservatory.DataSources.Libplctag.Models;
using libplctag;
using libplctag.DataTypes;

namespace CrudeObservatory.DataSources.Libplctag
{
    public class TagTypeMap
    {
        private readonly static Dictionary<TagType, TagConstructor> constructorMap;

        private delegate ITag TagConstructor();

        static TagTypeMap()
        {
            constructorMap = new Dictionary<TagType, TagConstructor>()
            {
                { TagType.Bool, () => new Tag<BoolPlcMapper, bool>() },
                { TagType.Bool1D, () => new Tag<BoolPlcMapper, bool[]>() },
                { TagType.Bool2D, () => new Tag<BoolPlcMapper, bool[,]>() },
                { TagType.Bool3D, () => new Tag<BoolPlcMapper, bool[,,]>() },
                { TagType.Dint, () => new Tag<DintPlcMapper, int>() },
                { TagType.Dint1D, () => new Tag<DintPlcMapper, int[]>() },
                { TagType.Dint2D, () => new Tag<DintPlcMapper, int[,]>() },
                { TagType.Dint3D, () => new Tag<DintPlcMapper, int[,,]>() },
                { TagType.Int, () => new Tag<IntPlcMapper, short>() },
                { TagType.Int1D, () => new Tag<IntPlcMapper, short[]>() },
                { TagType.Int2D, () => new Tag<IntPlcMapper, short[,]>() },
                { TagType.Int3D, () => new Tag<IntPlcMapper, short[,,]>() },
                { TagType.Lint, () => new Tag<LintPlcMapper, long>() },
                { TagType.Lint1D, () => new Tag<LintPlcMapper, long[]>() },
                { TagType.Lint2D, () => new Tag<LintPlcMapper, long[,]>() },
                { TagType.Lint3D, () => new Tag<LintPlcMapper, long[,,]>() },
                { TagType.Lreal, () => new Tag<LrealPlcMapper, double>() },
                { TagType.Lreal1D, () => new Tag<LrealPlcMapper, double[]>() },
                { TagType.Lreal2D, () => new Tag<LrealPlcMapper, double[,]>() },
                { TagType.Lreal3D, () => new Tag<LrealPlcMapper, double[,,]>() },
                { TagType.Real, () => new Tag<RealPlcMapper, float>() },
                { TagType.Real1D, () => new Tag<RealPlcMapper, float[]>() },
                { TagType.Real2D, () => new Tag<RealPlcMapper, float[,]>() },
                { TagType.Real3D, () => new Tag<RealPlcMapper, float[,,]>() },
                { TagType.Sint, () => new Tag<SintPlcMapper, sbyte>() },
                { TagType.Sint1D, () => new Tag<SintPlcMapper, sbyte[]>() },
                { TagType.Sint2D, () => new Tag<SintPlcMapper, sbyte[,]>() },
                { TagType.Sint3D, () => new Tag<SintPlcMapper, sbyte[,,]>() },
                { TagType.String, () => new Tag<StringPlcMapper, string>() },
                { TagType.String1D, () => new Tag<StringPlcMapper, string[]>() },
                { TagType.String2D, () => new Tag<StringPlcMapper, string[,]>() },
                { TagType.String3D, () => new Tag<StringPlcMapper, string[,,]>() },
            };
        }

        public static ITag GetDataSource(TagType type) => constructorMap[type].Invoke();
    }
}
