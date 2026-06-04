import type { ReactNode } from "react";

type IconButtonProps = {
  children: ReactNode;
  label: string;
  active?: boolean;
  onClick?: () => void;
};

export default function IconButton({ children, label, active = false, onClick }: IconButtonProps) {
  return (
    <button className={`icon-button ${active ? "active" : ""}`} onClick={onClick} title={label} type="button">
      <span aria-hidden="true">{children}</span>
      <span className="sr-only">{label}</span>
    </button>
  );
}
