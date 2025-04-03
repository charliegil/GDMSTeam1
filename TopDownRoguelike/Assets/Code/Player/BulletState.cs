
public static class BulletState
{
   public static float damage = 10;
   public static float size = 1.5f;
   
   public static void AddDamage(int moreDmg){
        damage+=moreDmg;
    }
    public static void AddSize(int moreSize){
        size+=moreSize;
    }

    public static void resetState(){
        damage = 10;
        size = 1.5f;
    }

    
}
