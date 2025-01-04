<template>
  <el-container class="layout-container">
    <el-header>
      <div class="header-content">
        <h2>OA System</h2>
        <div class="user-info">
          <span>{{ authStore.user?.email }}</span>
          <el-button type="text" @click="handleLogout">Logout</el-button>
        </div>
      </div>
    </el-header>

    <el-container>
      <el-main>
        <template v-if="authStore.isManager">
          <h3>Manager Dashboard</h3>
          <el-table :data="applications" style="width: 100%">
            <el-table-column prop="applicantEmail" label="Staff" />
            <el-table-column prop="applicationType" label="Type">
              <template #default="{ row }">
                {{ getTypeLabel(row.applicationType) }}
              </template>
            </el-table-column>
            <el-table-column prop="startDate" label="Start Date">
              <template #default="{ row }">
                {{ formatDate(row.startDate) }}
              </template>
            </el-table-column>
            <el-table-column prop="endDate" label="End Date">
              <template #default="{ row }">
                {{ formatDate(row.endDate) }}
              </template>
            </el-table-column>
            <el-table-column prop="status" label="Status">
              <template #default="{ row }">
                <el-tag :type="getStatusType(row.status)">
                  {{ getStatusLabel(row.status) }}
                </el-tag>
              </template>
            </el-table-column>
            <el-table-column prop="applicationReason" label="Reason" />
            <el-table-column label="Actions" width="200">
              <template #default="{ row }">
                <el-button-group v-if="row.status === 0">
                  <el-button size="small" type="success" @click="handleApprove(row)">
                    Approve
                  </el-button>
                  <el-button size="small" type="danger" @click="showRejectDialog(row)">
                    Reject
                  </el-button>
                </el-button-group>
              </template>
            </el-table-column>
          </el-table>
        </template>

        <template v-else>
          <h3>Staff Dashboard</h3>
          <el-button type="primary" @click="dialogVisible = true">
            New Leave Application
          </el-button>

          <el-table :data="applications" style="width: 100%; margin-top: 20px">
            <el-table-column prop="applicationType" label="Type">
              <template #default="{ row }">
                {{ getTypeLabel(row.applicationType) }}
              </template>
            </el-table-column>
            <el-table-column prop="startDate" label="Start Date">
              <template #default="{ row }">
                {{ formatDate(row.startDate) }}
              </template>
            </el-table-column>
            <el-table-column prop="endDate" label="End Date">
              <template #default="{ row }">
                {{ formatDate(row.endDate) }}
              </template>
            </el-table-column>
            <el-table-column prop="status" label="Status">
              <template #default="{ row }">
                <el-tag :type="getStatusType(row.status)">
                  {{ getStatusLabel(row.status) }}
                </el-tag>
              </template>
            </el-table-column>
            <el-table-column prop="comment" label="Comment" />
          </el-table>
        </template>
      </el-main>
    </el-container>

    <!-- New Application Dialog -->
    <el-dialog v-model="dialogVisible" title="New Leave Application">
      <el-form 
        :model="newApplication" 
        :rules="applicationRules"
        ref="applicationFormRef"
        label-width="120px"
      >
        <el-form-item label="Type" prop="applicationType">
          <el-select v-model="newApplication.applicationType">
            <el-option label="Annual Leave" :value="0" />
            <el-option label="Personal Leave" :value="1" />
          </el-select>
        </el-form-item>
        <el-form-item label="Date Range" prop="dateRange">
          <el-date-picker
            v-model="dateRange"
            type="daterange"
            range-separator="to"
            start-placeholder="Start Date"
            end-placeholder="End Date"
            :disabledDate="disabledDate"
            format="YYYY-MM-DD"
            value-format="YYYY-MM-DD"
            @change="handleDateChange"
          />
        </el-form-item>
        <el-form-item label="Reason" prop="applicationReason">
          <el-input
            v-model="newApplication.applicationReason"
            type="textarea"
            rows="3"
          />
        </el-form-item>
      </el-form>
      <template #footer>
        <el-button @click="dialogVisible = false">Cancel</el-button>
        <el-button type="primary" @click="submitApplication">Submit</el-button>
      </template>
    </el-dialog>

    <!-- Reject Dialog -->
    <el-dialog
      v-model="rejectDialogVisible"
      title="Reject Application"
      width="500px"
    >
      <el-form 
        :model="rejectForm"
        :rules="rejectRules"
        ref="rejectFormRef"
      >
        <el-form-item label="Reason" prop="reason">
          <el-input
            v-model="rejectForm.reason"
            type="textarea"
            rows="4"
            placeholder="Please enter rejection reason"
          />
        </el-form-item>
      </el-form>
      <template #footer>
        <el-button @click="rejectDialogVisible = false">Cancel</el-button>
        <el-button type="danger" @click="handleReject" :loading="rejectLoading">
          Confirm Reject
        </el-button>
      </template>
    </el-dialog>
  </el-container>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '../stores/auth'
import { ElMessage } from 'element-plus'
import moment from 'moment'
import api from '../utils/axios'
import { ApplicationStatus, getStatusLabel, getTypeLabel } from '../types'

const router = useRouter()
const authStore = useAuthStore()
const applications = ref([])
const dialogVisible = ref(false)
const dateRange = ref([])

const newApplication = ref({
  applicationType: 0,
  applicationReason: '',
  dateRange: [],
  startDate: '',
  endDate: ''
})

const rejectDialogVisible = ref(false)
const rejectLoading = ref(false)
const rejectForm = ref({
  reason: '',
  applicationId: ''
})

const applicationFormRef = ref()
const rejectFormRef = ref()

const applicationRules = {
  applicationType: [
    { required: true, message: 'Please select leave type', trigger: 'change' }
  ],
  applicationReason: [
    { required: true, message: 'Please enter reason', trigger: 'blur' },
    { min: 5, message: 'Reason must be at least 5 characters', trigger: 'blur' }
  ],
  dateRange: [
    { required: true, message: 'Please select date range', trigger: 'change', type: 'array' }
  ]
}

const rejectRules = {
  reason: [
    { required: true, message: 'Please enter rejection reason', trigger: 'blur' },
    { min: 5, message: 'Reason must be at least 5 characters', trigger: 'blur' }
  ]
}

const fetchApplications = async () => {
  try {
    const response = await api.get(
      authStore.isManager ? '/manager/leave-applications' : '/staff/leave-applications'
    )
    applications.value = response.data
  } catch (error) {
    ElMessage.error('Failed to fetch applications')
  }
}

const submitApplication = async () => {
  if (!applicationFormRef.value) return

  await applicationFormRef.value.validate(async (valid: boolean) => {
    if (valid && dateRange.value?.[0] && dateRange.value?.[1]) {
      try {
        const payload = {
          ...newApplication.value,
          startDate: `${dateRange.value[0]}T00:00:00`,
          endDate: `${dateRange.value[1]}T00:00:00`
        }

        await api.post('/staff/leave-applications', payload)
        ElMessage.success('Application submitted')
        dialogVisible.value = false
        fetchApplications()
        applicationFormRef.value.resetFields()
      } catch (error) {
        ElMessage.error('Failed to submit application')
      }
    }
  })
}

const handleApprove = async (application: any) => {
  try {
    await api.put(`/manager/leave-applications/${application.id}`, {
      status: ApplicationStatus.Approved
    })
    ElMessage.success('Application approved')
    fetchApplications()
  } catch (error) {
    ElMessage.error('Failed to approve application')
  }
}

const showRejectDialog = (application: any) => {
  rejectForm.value.applicationId = application.id
  rejectForm.value.reason = ''
  rejectDialogVisible.value = true
}

const handleReject = async () => {
  if (!rejectFormRef.value) return

  await rejectFormRef.value.validate(async (valid: boolean) => {
    if (valid) {
      rejectLoading.value = true
      try {
        await api.put(`/manager/leave-applications/${rejectForm.value.applicationId}`, {
          status: ApplicationStatus.Rejected,
          approverReason: rejectForm.value.reason
        })
        ElMessage.success('Application rejected')
        rejectDialogVisible.value = false
        fetchApplications()
        rejectFormRef.value.resetFields()
      } catch (error) {
        ElMessage.error('Failed to reject application')
      } finally {
        rejectLoading.value = false
      }
    }
  })
}

const handleLogout = () => {
  authStore.logout()
  router.push('/login')
}

const getStatusType = (status: ApplicationStatus) => {
  switch (status) {
    case ApplicationStatus.Pending:
      return 'warning'
    case ApplicationStatus.Approved:
      return 'success'
    case ApplicationStatus.Rejected:
      return 'danger'
    default:
      return 'info'
  }
}

const formatDate = (date: string) => {
  return moment(date).format('YYYY-MM-DD HH:mm')
}

const disabledDate = (time: Date) => {
  return time.getTime() < Date.now() - 8.64e7
}

const handleDateChange = (val: any) => {
  newApplication.value.dateRange = val
}

onMounted(() => {
  fetchApplications()
})
</script>

<style scoped>
.layout-container {
  min-height: 100vh;
}

.header-content {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.user-info {
  display: flex;
  align-items: center;
  gap: 10px;
}

.el-header {
  background-color: #fff;
  border-bottom: 1px solid #dcdfe6;
  padding: 0 20px;
}
</style> 