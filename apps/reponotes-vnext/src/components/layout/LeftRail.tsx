import {
  CheckSquare,
  Database,
  Files,
  GitBranch,
  LayoutTemplate,
  Search,
  Settings,
  Tag,
  Trash2,
  UserCircle
} from "lucide-react";

import type { RailItemId } from "../../types/reponotes";
import IconButton from "../common/IconButton";

type RailItem = {
  id: RailItemId;
  label: string;
  icon: typeof Files;
  enabled: boolean;
};

const primaryRailItems: RailItem[] = [
  { id: "files", label: "Repository", icon: Files, enabled: true },
  { id: "search", label: "Search", icon: Search, enabled: true },
  { id: "links", label: "Links / Graph", icon: GitBranch, enabled: false },
  { id: "tags", label: "Tags", icon: Tag, enabled: false },
  { id: "tasks", label: "Tasks", icon: CheckSquare, enabled: false },
  { id: "templates", label: "Templates", icon: LayoutTemplate, enabled: false },
  { id: "entities", label: "Entities", icon: Database, enabled: false }
];

const secondaryRailItems: RailItem[] = [
  { id: "trash", label: "Trash", icon: Trash2, enabled: true },
  { id: "settings", label: "Settings", icon: Settings, enabled: true },
  { id: "profile", label: "Profile", icon: UserCircle, enabled: false }
];

type LeftRailProps = {
  activeRailItem: RailItemId;
  onSelectRailItem: (item: RailItemId) => void;
};

export default function LeftRail({ activeRailItem, onSelectRailItem }: LeftRailProps) {
  return (
    <aside className="left-rail" aria-label="activity bar">
      <div className="rail-top">
        <RailStack activeRailItem={activeRailItem} items={primaryRailItems} onSelectRailItem={onSelectRailItem} />
      </div>
      <RailStack activeRailItem={activeRailItem} items={secondaryRailItems} onSelectRailItem={onSelectRailItem} />
    </aside>
  );
}

function RailStack({
  activeRailItem,
  items,
  onSelectRailItem
}: {
  activeRailItem: RailItemId;
  items: RailItem[];
  onSelectRailItem: (item: RailItemId) => void;
}) {
  return (
    <div className="rail-stack">
      {items.map((item) => {
        const Icon = item.icon;

        return (
          <IconButton
            key={item.id}
            active={activeRailItem === item.id}
            disabled={!item.enabled}
            label={item.enabled ? item.label : `${item.label} (future)`}
            onClick={() => onSelectRailItem(item.id)}
          >
            <Icon aria-hidden="true" size={18} strokeWidth={1.9} />
          </IconButton>
        );
      })}
    </div>
  );
}
