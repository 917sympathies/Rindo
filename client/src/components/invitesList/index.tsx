"use client";
import { IInvitation } from "@/types";
import { useState, useEffect } from "react";
import { useParams } from "next/navigation";
import {
  Table,
  TableBody,
  TableCaption,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuGroup,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuPortal,
  DropdownMenuSeparator,
  DropdownMenuShortcut,
  DropdownMenuSub,
  DropdownMenuSubContent,
  DropdownMenuSubTrigger,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import { EllipsisVertical } from "lucide-react";

interface IInvitationDto {
  id: string;
  sender: string;
  user: string;
}

export default function InvitesList() {
  const { id: projectId } = useParams<{ id: string }>();
  const [invites, setInvites] = useState<IInvitationDto[]>(
    [] as IInvitationDto[]
  );

  useEffect(() => {
    getInvitations(projectId);
  }, []);

  const getInvitations = async (id: string) => {
    const response = await fetch(
      `http://localhost:5000/api/invitation/project?projectId=${id}`,
      {
        method: "GET",
        headers: { "Content-Type": "application/json" },
        credentials: "include",
      }
    );
    const data = await response.json();
    setInvites(data);
  };

  const deleteInvite = async (id: string) => {
    const response = await fetch(
        `http://localhost:5000/api/invitation/${id}`,
        {
          method: "DELETE",
          headers: { "Content-Type": "application/json" },
          credentials: "include",
        }
    );
    if(response.ok) setInvites(invites.filter(inv => inv.id != id));
  }

  return (
    <div className="rounded-lg border w-[60%]">
      <Table className="">
        {/* <TableCaption>Список задач проекта</TableCaption> */}
        <TableHeader>
          <TableRow className="w-full">
            <TableHead className="w-[47%]">Пользователь</TableHead>
            <TableHead className="w-[47%]">Отправитель</TableHead>
            <TableHead className="w-[6%]"></TableHead>
          </TableRow>
        </TableHeader>
        <TableBody>
          {invites?.map((invite) => (
            <TableRow>
              <TableCell className="font-medium overflow-hidden">
                {invite.user}
              </TableCell>
              <TableCell className="font-medium">{invite.sender}</TableCell>
              <TableCell className="w-full">
                <DropdownMenu>
                  <DropdownMenuTrigger asChild>
                    <EllipsisVertical
                      className="p-1 hover:bg-gray-100 rounded-lg ease-in-out duration-300 text-gray-500"
                      size={24}
                    />
                  </DropdownMenuTrigger>
                  <DropdownMenuContent className="w-[20%] dark:bg-[#111] dark:border-black/20">
                    {/* <DropdownMenuLabel>username</DropdownMenuLabel>
                    <DropdownMenuSeparator className="dark:bg-black/20" /> */}
                    <DropdownMenuGroup>
                      <DropdownMenuItem>
                        <div className="w-full self-center" onClick={() => deleteInvite(invite.id)}>Удалить</div>
                      </DropdownMenuItem>
                    </DropdownMenuGroup>
                  </DropdownMenuContent>
                </DropdownMenu>
              </TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>
    </div>
  );
}
