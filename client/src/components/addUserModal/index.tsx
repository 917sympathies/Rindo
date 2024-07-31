"use client";
import { Button } from "../ui/button";
import { Input } from "../ui/input";
import { User2, X } from "lucide-react";
import { useState } from "react";
import { useParams } from "next/navigation";
import { InviteUserToProject } from "@/requests";

interface ModalProps {
  onClose: () => void;
}

export default function AddUserModal({ onClose }: ModalProps) {
  const [username, setUsername] = useState<string>("");
  const { id } = useParams<{ id: string }>();

  const handleInviteUser = async () => {
    const response = await InviteUserToProject(id, username);
    setUsername("");
    onClose();
  };

  return (
    <div className="flex flex-col justify-center w-full rounded-[8px] p-[10px] bg-white dark:bg-[#111] dark:border-black/20 text-black dark:text-white">
      <div className="flex flex-row items-center justify-between m-[0.2rem] mb-[1rem]">
        <div className="flex flex-row items-center">
          <User2 size={24} />
          <div className="ml-[0.2rem]">Добавить пользователя</div>
        </div>
        <X
          size={24}
          onClick={onClose}
          className="p-1 rounded-full hover:bg-black/10 ease-in-out duration-300"
        ></X>
      </div>
      <div className="flex flex-row items-center justify-between gap-2">
        <Input
          className="focus-visible:outline-none focus-visible:ring-0 focus-visible:ring-slate-950 focus-visible:ring-offset-0 dark:border-black/40"
          placeholder="Имя пользователя"
          value={username}
          onChange={(e) => setUsername(e.target.value)}
        />
        <Button
          className="text-white bg-blue-500 hover:bg-blue-800 mr-[0.4rem]"
          onClick={() => handleInviteUser()}
        >
          Отправить
        </Button>
      </div>
    </div>
  );
}
