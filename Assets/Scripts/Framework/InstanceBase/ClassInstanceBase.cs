public class ClassInstanceBase<T> where T : new()
{
    private static T intance;
    public static T Instance {
        get {
            if (intance == null) {
                intance = new T();
            }
            return intance;
        }
    }
}