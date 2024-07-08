"use client";
import { useState, useEffect, Dispatch, SetStateAction } from "react";
import Link from "next/link";
import Chat from "../chat";
import { Sheet, SheetContent } from "../ui/sheet";
import { useParams } from "next/navigation";

interface HeaderProps {
  setIsSelectorVisible: Dispatch<SetStateAction<boolean>>;
  isChatActive: boolean;
  setIsChatActive: Dispatch<SetStateAction<boolean>>;
}

interface IProject {
  name: string;
  chatId: string;
  ownerId: string;
}

export default function Header({
  setIsSelectorVisible,
  isChatActive,
  setIsChatActive,
}: HeaderProps) {
  const { id } = useParams<{ id: string }>();
  const [isOwner, setIsOwner] = useState<boolean>(false);
  const [project, setProject] = useState<IProject | null>(null);

  useEffect(() => {
    const userId = localStorage.getItem("userId");
    if (userId == project?.ownerId) setIsOwner(true);
    else setIsOwner(false);
  }, [project]);

  useEffect(() => {
    const getProjectInfo = async (id: string) => {
      const response = await fetch(
        `http://localhost:5000/api/project/${id}/header`,
        {
          method: "GET",
          headers: { "Content-Type": "application/json" },
          credentials: "include",
        }
      );
      const data = await response.json();
      setProject(data);
    };
    getProjectInfo(id);
  }, [id]);

  return (
    <div className={isChatActive ? "w-[calc(100%-18vw)] m-0 py-[20px] px-[10px] flex flex-row justify-evenly items-center ease-out duration-300 font-medium border-b border-b-black/10"
      : "w-full m-0 py-[20px] px-[10px] flex flex-row justify-evenly items-center ease-out duration-300 font-medium border-b border-b-black/10"
    }
    >
      <Link
        className="m-[10px] text-[1.4rem]"
        href={`/project/${id}/board`}
        onClick={() => setIsSelectorVisible(true)}
      >
        {project && project.name}
      </Link>
      <div
        className="cursor-pointer"
        onClick={() => setIsChatActive(!isChatActive)}
      >
        <h3 className="text-[1.2rem]">Чат проекта</h3>
      </div>
      <Link
        className="text-[1.2rem]"
        href={`/project/${id}/board`}
        onClick={() => setIsSelectorVisible(true)}
      >
        <h3>Задачи</h3>
      </Link>
      {isOwner ? (
        <Link
          className="text-[1.2rem]"
          href={`/project/${id}/settings/general`}
          onClick={() => setIsSelectorVisible(false)}
        >
          <h3>Настройки</h3>
        </Link>
      ) : (
        <div></div>
      )}
      <Sheet key={"right"} open={isChatActive} modal={false}>
        <SheetContent
          className="h-screen top-0 right-0 left-auto mt-0 w-[400px] rounded-none"
          side={"right"}
        >
          <Chat
            isActive={isChatActive}
            setIsChatActive={setIsChatActive}
            chatId={project?.chatId}
            projectName={project?.name}
          />
        </SheetContent>
      </Sheet>
    </div>
  );
}
