using Animations;

public static class AnimationExtensions{
    public static T OfType<T>(this IAnimatable obj){
        if(obj is T t){
            return t;
        }
        return default;
    }
}