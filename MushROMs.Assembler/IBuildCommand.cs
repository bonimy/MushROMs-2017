namespace MushROMs.Assembler
{
    public interface IBuildCommand
    {
        byte[] ToData();
        void Execute(Builder builder);
    }
}
