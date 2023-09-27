import { useEffect, useState } from "react";
import { completeChore, getMyChores } from "../../managers/choreManager";
import { Button, Table } from "reactstrap";

export const UserChoreList = ({ loggedInUser }) => {
    const [chores, setChores] = useState([]);

    const getProfileWithChores = () => {
        getMyChores(loggedInUser.id).then(setChores);
    }

    const handleComplete = (id) => {
        completeChore(id, loggedInUser.id)
            .then(getProfileWithChores);
    }

    useEffect(() => {
        getProfileWithChores();
    }, []);

    if (!chores) {
        return
    }

    return (
        <div className="container">
            <div className="sub-menu bg-light">
                <h4>Chores Assignments</h4>
            </div>
            <Table>
                <thead>
                    <tr>
                        <th>#</th>
                        <th>Name</th>
                        <th>Difficulty</th>
                        <th>Frequency</th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    {chores.map((ca) => {
                        if (ca.chore.overdueChore === true) {
                            return (
                                <tr key={`chores-${ca.chore.id}`}>
                                    <th scope="row">{ca.chore.id}</th>
                                    <td style={{ color: "red" }}>{ca.chore.name}</td>
                                    <td>{ca.chore.difficulty}</td>
                                    <td>Every {ca.chore.choreFrequencyDays} Days</td>
                                    <td>
                                        <Button
                                            color="success"
                                            onClick={() => {
                                                handleComplete(ca.chore.id);
                                            }}
                                        >
                                            Complete
                                        </Button>
                                    </td>
                                </tr>
                            )
                        }
                    })}
                </tbody>
            </Table>
        </div>
    )
}