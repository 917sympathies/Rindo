import { Dispatch, SetStateAction } from "react";

interface IEditorProps {
  desc: string | undefined;
  placeholder?: string;
  setDesc: Dispatch<SetStateAction<string>>;
  styles?: string;
}

export default function Editor({
  desc,
  placeholder,
  setDesc,
  styles,
}: IEditorProps) {
  return (
    <div className="w-full">
      <textarea
        className={
          "resize-none border border-slate-200 font-sm tracking-normal p-2.5 rounded-lg h-40 w-full focus:outline-none " +
          styles
        }
        placeholder={placeholder ? placeholder : "Описание..."}
        value={desc}
        onChange={(e) => setDesc(e.target.value)}
      ></textarea>
    </div>
  );
}
