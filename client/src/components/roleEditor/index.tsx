"use client";
import { useState, Dispatch, SetStateAction } from "react";
import { Dialog, DialogContent } from "../ui/dialog"
import { Switch } from "../ui/switch";
import { Button } from "../ui/button";
import { Input } from "../ui/input";
import { Label } from "../ui/label";
import { IRole } from "@/types";
import { deleteRole } from "@/requests";

interface Props {
  selectedRole: IRole;
  setSelectedRole: Dispatch<SetStateAction<IRole>>;
  handleSaveRole: () => void;
  doFetch: () => void;
}

export default function RoleEditor({
  selectedRole,
  setSelectedRole,
  handleSaveRole,
  doFetch: doFetch
}: Props) {
  const [isModalOpen, setIsModalOpen] = useState<boolean>(false);

  const handleDeleteRole = async () => {
    const response = await deleteRole(selectedRole.id);
    setIsModalOpen(false);
    doFetch();
  }

  return (
    <>
      <div className="bg-[#90e0ef] rounded-r-lg rounded-bl-lg dark:bg-black/30">
        <div className="p-4">
          <Input
            className="rounded-full focus-visible:outline-none focus-visible:ring-0 focus-visible:ring-slate-950 focus-visible:ring-offset-0 dark:border-black/20"
            placeholder="Название роли"
            value={selectedRole.name}
            onChange={(e) =>
              setSelectedRole({
                ...selectedRole,
                name: e.target.value,
              })
            }
          ></Input>
          <div className="flex flex-row items-center pt-4 pr-4 gap-4">
            <Switch
              id="canAddTask"
              checked={selectedRole ? selectedRole.canAddTask : false}
              onCheckedChange={(value: boolean) =>
                setSelectedRole({
                  ...selectedRole,
                  canAddTask: value,
                })
              }
            ></Switch>
            <Label htmlFor="canAddTask">Может добавлять задачи в проект</Label>
          </div>
          <div className="flex flex-row items-center pt-4 pr-4 gap-4">
            <Switch
              id="canDeleteTask"
              checked={selectedRole ? selectedRole.canDeleteTask : false}
              onCheckedChange={(value: boolean) =>
                setSelectedRole({
                  ...selectedRole,
                  canDeleteTask: value,
                })
              }
            ></Switch>
            <Label htmlFor="canDeleteTask">
              Может удалять задачи из проекта
            </Label>
          </div>
          <div className="flex flex-row items-center pt-4 pr-4 gap-4">
            <Switch
              id="canAddStage"
              checked={selectedRole ? selectedRole.canAddStage : false}
              onCheckedChange={(value: boolean) =>
                setSelectedRole({
                  ...selectedRole,
                  canAddStage: value,
                })
              }
            ></Switch>
            <Label htmlFor="canAddStage">Может добавлять стадии</Label>
          </div>
          <div className="flex flex-row items-center pt-4 pr-4 gap-4">
            <Switch
              id="canDeleteStage"
              checked={selectedRole ? selectedRole.canDeleteStage : false}
              onCheckedChange={(value: boolean) =>
                setSelectedRole({
                  ...selectedRole,
                  canDeleteStage: value,
                })
              }
            ></Switch>
            <Label htmlFor="canDeleteStage">Может удалять стадии</Label>
          </div>
          <div className="flex flex-row items-center pt-4 pr-4 gap-4">
            <Switch
              id="canInviteUser"
              checked={selectedRole ? selectedRole.canInviteUser : false}
              onCheckedChange={(value: boolean) =>
                setSelectedRole({
                  ...selectedRole,
                  canInviteUser: value,
                })
              }
            ></Switch>
            <Label htmlFor="canInviteUser">Может приглашать пользователей</Label>
          </div>
          <div className="flex flex-row items-center pt-4 pr-4 gap-4">
            <Switch
              id="canModifyTask"
              checked={selectedRole ? selectedRole.canModifyTask : false}
              onCheckedChange={(value: boolean) =>
                setSelectedRole({
                  ...selectedRole,
                  canModifyTask: value,
                })
              }
            ></Switch>
            <Label htmlFor="canModifyTask">Может редактировать задачи</Label>
          </div>
          <div className="flex flex-row gap-2">
            <Button
              className="w-full mt-4 text-black bg-gray-200 hover:bg-white ease-in-out duration-300"
              onClick={() => handleSaveRole()}
            >
              Сохранить изменения
            </Button>
            <Button
              className="w-full mt-4 text-white bg-red-500 hover:bg-red-400 ease-in-out duration-300"
              onClick={() => setIsModalOpen(true)}>
              Удалить
            </Button>
          </div>
        </div>
      </div>
      <Dialog open={isModalOpen}>
        <DialogContent>
          <div
            style={{
              display: "flex",
              flexDirection: "column",
              justifyContent: "center",
              backgroundColor: "white",
              width: "100%",
              borderRadius: "8px",
              padding: "10px",
              gap: 8,
            }}
          >
            <Label className="flex text-black gap-1">
              Вы действительно хотите удалить роль {<div className="font-[800]">{selectedRole.name}</div>}?
            </Label>
            <div style={{ display: "flex", alignSelf: "center" }}>
              <Button
                onClick={() => handleDeleteRole()}
                style={{
                  color: "white",
                  backgroundColor: "green",
                  marginRight: "0.4rem",
                }}
              >
                Да
              </Button>
              <Button
                onClick={() => setIsModalOpen(false)}
                style={{ color: "white", backgroundColor: "red" }}
              >
                Нет
              </Button>
            </div>
          </div>
        </DialogContent>
      </Dialog>
    </>
  );
}
