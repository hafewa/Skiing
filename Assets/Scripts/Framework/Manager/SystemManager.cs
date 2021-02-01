public class SystemManager : IManager
{
    public GuideSystem guideSystem;

    public override void OnInit() {
        base.OnInit();

        guideSystem = new GuideSystem();

        guideSystem.OnInit();
    }

    public override void OnRelease() {
        base.OnRelease();

        guideSystem.OnRelease();
    }

    public override void OnUpdate() {
        base.OnUpdate();

        guideSystem.OnUpdate();
    }
}