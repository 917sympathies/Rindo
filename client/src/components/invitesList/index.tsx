"use client";
import { useState, useEffect } from "react";
import { useParams } from "next/navigation";
import {
    Table,
    TableBody,
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
    DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import { EllipsisVertical } from "lucide-react";
import { deleteInvite as deleteInviteRequest, getInvitesByProjectId } from "@/requests";

interface InvitationProjectDto {
    id: string;
    senderUsername: string;
    recipientUsername: string;
}

export default function InvitesList() {
    const { id: projectId } = useParams<{ id: string }>();
    const [invites, setInvites] = useState<InvitationProjectDto[]>([]);

    useEffect(() => {
        getInvitations(projectId);
    }, []);

    const getInvitations = async (id: string) => {
        const response = await getInvitesByProjectId(id);
        const data = response.data;
        setInvites(data);
    };

    const deleteInvite = async (id: string) => {
        await deleteInviteRequest(id)
            .then(response => {
                if(response.status === 200) {
                    setInvites(invites.filter(inv => inv.id != id));
                }
            })
    }

    return (
        <div className="rounded-lg border w-[60%]">
            <Table className="">
                <TableHeader>
                    <TableRow className="w-full">
                        <TableHead className="w-[47%]">Приглашенный пользователь</TableHead>
                        <TableHead className="w-[47%]">Отправитель</TableHead>
                        <TableHead className="w-[6%]"></TableHead>
                    </TableRow>
                </TableHeader>
                <TableBody>
                    {invites?.map((invite) => (
                        <TableRow key={invite.id}>
                            <TableCell className="font-medium overflow-hidden">
                                {invite.recipientUsername}
                            </TableCell>
                            <TableCell className="font-medium">{invite.senderUsername}</TableCell>
                            <TableCell className="w-full">
                                <DropdownMenu>
                                    <DropdownMenuTrigger asChild>
                                        <EllipsisVertical
                                            className="p-1 hover:bg-gray-100 rounded-lg ease-in-out duration-300 text-gray-500"
                                            size={24}
                                        />
                                    </DropdownMenuTrigger>
                                    <DropdownMenuContent className="w-[20%] dark:bg-[#111] dark:border-black/20">
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
