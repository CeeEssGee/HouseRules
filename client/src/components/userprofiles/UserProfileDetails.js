import { useEffect, useState } from "react"
import { getUserById } from "../../managers/userProfileManager"
import { useParams } from "react-router-dom"
import { Table } from "reactstrap"


export const UserProfileDetails = () => {
    const [userProfile, setUserProfile] = useState(null)

    const { userId } = useParams();

    useEffect(() => {
        getUserById(userId).then(setUserProfile)
    }, [])

    if (!userProfile) {
        return null;
    }

    return (
        <div className="container">
            <h2>{userProfile.firstName} {userProfile.lastName}</h2>
            <Table>
                <tbody>
                    <tr>
                        <th scope="row">First Name</th>
                        <td>{userProfile.firstName}</td>
                    </tr>
                    <tr>
                        <th scope="row">Last name</th>
                        <td>{userProfile.lastName}</td>
                    </tr>
                    <tr>
                        <th scope="row">Address</th>
                        <td>{userProfile.address}</td>
                    </tr>
                    <tr>
                        <th scope="row">Email</th>
                        <td>{userProfile.identityUser.email}</td>
                    </tr>
                    <tr>
                        <th scope="row">User Name</th>
                        <td>{userProfile.identityUser.userName}</td>
                    </tr>
                </tbody>
            </Table>
            <h5>Assiged Chores</h5>
            <Table>
                <thead>
                    <tr>
                        <th>Name</th>
                        <th>Difficulty</th>
                        <th>Frequency</th>
                    </tr>
                </thead>
                <tbody>
                    {userProfile.choreAssignments.map((ca) => (
                        <tr key={`choreAssignment--${ca.id}`}>
                            <td>{ca.chore.name}</td>
                            <td>{ca.chore.difficulty}</td>
                            <td>{ca.chore.choreFrequencyDays}</td>
                        </tr>
                    ))}
                </tbody>
            </Table>
            <h5>Completed Chores</h5>
            <Table>
                <thead>
                    <tr>
                        <th>Name</th>
                        <th>Difficulty</th>
                        <th>Frequency</th>
                        <th>Date Completed</th>
                    </tr>
                </thead>
                <tbody>
                    {userProfile.choreCompletions.map((cc) => (
                        <tr key={`choreCompletion--${cc.id}`}>
                            <td>{cc.chore.name}</td>
                            <td>{cc.chore.difficulty}</td>
                            <td>{cc.chore.choreFrequencyDays}</td>
                            <td>{new Date(cc.completedOn).toLocaleDateString('en-US')}</td>
                        </tr>
                    ))}
                </tbody>
            </Table>

        </div>
    )

}
