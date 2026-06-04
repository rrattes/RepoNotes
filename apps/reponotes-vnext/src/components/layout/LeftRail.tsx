import IconButton from "../common/IconButton";
import { railItems } from "../../data/mockRepository";

export default function LeftRail() {
  return (
    <aside className="left-rail" aria-label="workspace navigation">
      <div className="rail-stack">
        {railItems.slice(0, -1).map((item) => (
          <IconButton key={item.id} label={item.label} active={item.id === "files"}>
            {item.icon}
          </IconButton>
        ))}
      </div>
      <IconButton label="Settings">{railItems[railItems.length - 1].icon}</IconButton>
    </aside>
  );
}
