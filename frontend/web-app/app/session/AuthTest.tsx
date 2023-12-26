'use client'
import React from 'react'
import { UpdateAuctionTest } from '../actions/auctionActions'
import { Button } from 'flowbite-react'

export default function AuthTest() {
    const [loading, setLoading] = React.useState(false)
    const [result, setResult] = React.useState<any>(null)

    function doUpdate()
    {
        setResult(null)
        setLoading(true)
        UpdateAuctionTest()
            .then((res) => {
                setResult(res)
            })
            .catch((err) => {
                setResult(err)
            })
            .finally(() => {
                setLoading(false)
            })
    }
  return (
    <div className='flex items-center gap-4'>
            <Button outline isProcessing={loading} onClick={doUpdate}>
                Test auth
            </Button>
            <div>
                {JSON.stringify(result, null, 2)}
            </div>
        </div>
  )
}
