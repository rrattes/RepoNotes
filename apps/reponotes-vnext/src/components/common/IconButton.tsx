import type { ReactNode } from "react";

type IconButtonProps = {
  children: ReactNode;
  label: string;
  active?: boolean;
  disabled?: boolean;
  onClick?: () => void;
};

export default function IconButton({ children, label, active = false, disabled = false, onClick }: IconButtonProps) {
  return (
    <button
      aria-disabled={disabled}
      className={`icon-button ${active ? "active" : ""} ${disabled ? "disabled" : ""}`}
      disabled={disabled}
      onClick={disabled ? undefined : onClick}
      title={label}
      type="button"
    >
      <span aria-hidden="true">{children}</span>
      <span className="sr-only">{label}</span>
    </button>
  );
}
