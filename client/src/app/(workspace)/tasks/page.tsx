import UserTasks from "@/components/usertasks";

export default function Page() {

    return (
      <>
        <div className="flex flex-col p-4 gap-4">
          <span style={{display: "flex", alignSelf: "center", justifyContent: "center", fontSize: "2rem", fontFamily: "inherit"}}>Ваши задачи</span>
          <UserTasks/>
        </div>
      </>
    );
}